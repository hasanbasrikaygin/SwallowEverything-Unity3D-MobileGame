using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Boxophobic.StyledGUI;

public class WeaponChanger : MonoBehaviour
{
    private Vector3 centerPosition = Vector3.zero; // Merkez pozisyon

    public GameObject noInternetPanel;

    public List<Weapon> weapons;
    public Button rightButton;
    public Button leftButton;
    public Button returnHomeButton;
    public float spacing = 5f; // Silahlar arasýndaki boþluk
    public float transitionSpeed = 5f; // Geçiþ hýzý
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponNameColorCanvas;

    private int selectedWeaponIndex = 0;
    private List<GameObject> weaponObjects = new List<GameObject>();
    private bool isTransitioning = false;
    private bool isRightButtonDown = false;
    private bool isLeftButtonDown = false;
    private bool isTurnButtonDown = false;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI bulletSpeedText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI fireRateText;

    public Image damageBar;
    public Image bulletSpeedBar;
    public Image rangeBar;
    public Image fireRateBar;

    public float weaponDamage, weaponMaxDamage = 100;
    public float weaponBulletSpeed, weaponMaxBulletSpeed = 100;
    public float weaponRange, weaponMaxRange = 100;
    public float weaponFireRate, weaponMaxFireRate = 100;

    public float lerpSpeed;
    public Image[] damageBarPoints;
    public Image[] bulletSpeedPoints;
    public Image[] rangeBarPoints;
    public Image[] fireRateBarPoints;

    public float originalScale = 1f;
    public float enlargedScale = 2f;

    public GameObject lockImage; // Kilit resmini ekrana sürükleyip býrakabilirsiniz
    public TextMeshProUGUI lockCountText;
    public GameObject styleButton;
    public WeaponAdsManager adsManager;
    public HomeScreenAnimation homeScreenAnimation;
    void Start()
    {
        selectedWeaponIndex = PlayerPrefs.GetInt("SelectedWeaponIndex", 0);

        PopulateWeapon();
        InitializeWeaponLockCounts();
        //PlayerPrefs.SetInt(weapons[0].gunID + "_LockCount", 3); // Ýlk silah kilitli
        //PlayerPrefs.SetInt(weapons[1].gunID + "_LockCount", 0); // Ýkinci silah kilitsiz
        rightButton.onClick.AddListener(OnRightButtonDown);
        leftButton.onClick.AddListener(OnLeftButtonDown);
        returnHomeButton.onClick.AddListener(OnTurnHomeButtonDown);
        LoadAndApplyWeaponColors();
        UpdateTargetWeapon();
        CheckWeaponLock();
        noInternetPanel.SetActive(false);
    }
    void InitializeWeaponLockCounts()
    {
        // Sadece ilk kez baþlatýldýðýnda PlayerPrefs'lere deðerleri kaydedin
        for (int i = 0; i < weapons.Count; i++)
        {
            if (!PlayerPrefs.HasKey(weapons[i].gunID + "_LockCount"))
            {
                if (i <=3) // Ýlk silah kilitli
                {
                    PlayerPrefs.SetInt(weapons[i].gunID + "_LockCount", 0);
                }
                else // Diðer silahlar için kilit durumu
                {
                    PlayerPrefs.SetInt(weapons[i].gunID + "_LockCount", 2);
                }
            }
        }
    }
    void LoadAndApplyWeaponColors()
    {
        foreach (var weapon in weapons)
        {
            MeshRenderer renderer = weapon.weapon.GetComponent<MeshRenderer>();

            if (renderer == null)
            {
                continue;
            }

            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                string savedMaterialName = PlayerPrefs.GetString(weapon.gunID + "-" + i);

                if (!string.IsNullOrEmpty(savedMaterialName))
                {
                    Material loadedMaterial = Resources.Load<Material>("Materials/" + savedMaterialName);

                    if (loadedMaterial != null)
                    {
                        materials[i] = loadedMaterial;
                    }
                }
            }

            renderer.materials = materials;
        }
    }

    void UpdateWeaponPositions()
    {
        for (int i = 0; i < weaponObjects.Count; i++)
        {
            float offset = (i - selectedWeaponIndex) * spacing;
            weaponObjects[i].transform.localPosition = centerPosition + new Vector3(offset, 0, 0);
            weaponObjects[i].transform.rotation = Quaternion.Euler(21.5f, 45.8f, 3.5f);
        }
    }

    void PopulateWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weaponObjects.Add(weapons[i].weapon);
        }
        UpdateWeaponPositions();
        UpdateTargetWeapon();
    }

    bool DisplayBarPoint(float value, int pointNumber)
    {
        return ((pointNumber * 10) >= value);
    }

    void BarFiller(Image bar, float currentValue, float maxValue, Image[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].enabled = !DisplayBarPoint(currentValue, i);
        }
    }

    public void ToggleAllWeapons()
    {
        for (int i = 0; i < weaponObjects.Count; i++)
        {
            StartCoroutine(TurnWeaponSmoothly(weaponObjects[i], Quaternion.Euler(332.7f, 212f, 3.2f)));
            if (i != selectedWeaponIndex)
            {
                weaponObjects[i].SetActive(false);
            }
        }
    }

    public void ShowAllWeapons()
    {
        for (int i = 0; i < weaponObjects.Count; i++)
        {
            weaponObjects[i].SetActive(true);
            StartCoroutine(TurnWeaponSmoothly(weaponObjects[i], Quaternion.Euler(21.5f, 45.8f, 3.5f)));
        }
    }

    IEnumerator TurnWeaponSmoothly(GameObject weaponObject, Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = weaponObject.transform.rotation;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 2f;
            weaponObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }
    }

    void FixedUpdate()
    {
        UpdateWeaponStats();
        lerpSpeed = 3f * Time.deltaTime;
        EnlargeSelectedWeapon();

        if (isRightButtonDown)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Count;
            StartCoroutine(TransitionWeapons(selectedWeaponIndex));
            isRightButtonDown = false;
        }

        if (isLeftButtonDown)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Count) % weapons.Count;
            StartCoroutine(TransitionWeapons(selectedWeaponIndex));
            isLeftButtonDown = false;
        }

        if (isTurnButtonDown)
        {
            TurnHomeButton();
            isTurnButtonDown = false;
        }
    }

    IEnumerator TransitionWeapons(int targetIndex)
    {
        isTransitioning = true;
        float elapsedTime = 0f;
        Vector3[] startPositions = new Vector3[weaponObjects.Count];

        for (int i = 0; i < weaponObjects.Count; i++)
        {
            startPositions[i] = weaponObjects[i].transform.localPosition;
        }

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed * 8f;
            for (int i = 0; i < weaponObjects.Count; i++)
            {
                float offset = (i - selectedWeaponIndex) * spacing;
                Vector3 targetPosition = centerPosition + new Vector3(offset, 0, 0);
                weaponObjects[i].transform.localPosition = Vector3.Lerp(startPositions[i], targetPosition, elapsedTime);
            }
            yield return null;
        }
        isTransitioning = false;
        UpdateTargetWeapon();
        CheckWeaponLock();
    }

    void UpdateWeaponStats()
    {
        damageText.text = "" + weapons[selectedWeaponIndex].damage;
        bulletSpeedText.text = "" + weapons[selectedWeaponIndex].bulletSpeed;
        rangeText.text = "" + weapons[selectedWeaponIndex].range;
        fireRateText.text = "" + weapons[selectedWeaponIndex].fireRate;
        weaponName.text = weapons[selectedWeaponIndex].name;
        weaponNameColorCanvas.text = weapons[selectedWeaponIndex].name;

        BarFiller(damageBar, weapons[selectedWeaponIndex].damage, weaponMaxDamage, damageBarPoints);
        BarFiller(bulletSpeedBar, weapons[selectedWeaponIndex].bulletSpeed, weaponMaxBulletSpeed, bulletSpeedPoints);
        BarFiller(rangeBar, weapons[selectedWeaponIndex].range, weaponMaxRange, rangeBarPoints);
        BarFiller(fireRateBar, weapons[selectedWeaponIndex].fireRate, weaponMaxFireRate, fireRateBarPoints);

        SelectedWeapon.gunID = weapons[selectedWeaponIndex].gunID;
    }

    void UpdateTargetWeapon()
    {
        transform.position = weaponObjects[selectedWeaponIndex].transform.position;
        PlayerPrefs.SetInt("SelectedWeaponIndex", selectedWeaponIndex);
    }

    void EnlargeSelectedWeapon()
    {
        weaponObjects[selectedWeaponIndex].transform.localScale = Vector3.one * enlargedScale;

        for (int i = 0; i < weaponObjects.Count; i++)
        {
            if (i != selectedWeaponIndex)
            {
                weaponObjects[i].transform.localScale = Vector3.one * originalScale;
            }
        }
    }

    void OnRightButtonDown()
    {
        if (!isTransitioning)
        {
            isRightButtonDown = true;
        }
    }

    void OnLeftButtonDown()
    {
        if (!isTransitioning)
        {
            isLeftButtonDown = true;
        }
    }

    void OnTurnHomeButtonDown()
    {
        if (!isTransitioning)
        {
            isTurnButtonDown = true;
        }
    }

    void TurnHomeButton()
    {
        if (GetWeaponLockCount(weapons[selectedWeaponIndex].gunID) > 0)
        {
            Debug.Log("Silah kilitli, kaydedilmeyecek.");
            homeScreenAnimation.OnButtonClick(lockImage);
            return;
        }

        PlayerPrefs.SetString("Name", weapons[selectedWeaponIndex].name);
        PlayerPrefs.SetString("GunID", weapons[selectedWeaponIndex].gunID);
        PlayerPrefs.SetFloat("Damage", weapons[selectedWeaponIndex].damage);
        PlayerPrefs.SetFloat("BulletSpeed", weapons[selectedWeaponIndex].bulletSpeed);
        PlayerPrefs.SetFloat("Range", weapons[selectedWeaponIndex].range);
        PlayerPrefs.SetFloat("FireRate", weapons[selectedWeaponIndex].fireRate);
        PlayerPrefs.SetInt("SelectedWeaponIndex", selectedWeaponIndex);

        SceneManager.LoadScene("Home");
    }

    int GetWeaponLockCount(string gunID)
    {
        return PlayerPrefs.GetInt(gunID + "_LockCount", 0); // Varsayýlan olarak 0 döner
    }

    void SetWeaponLockCount(string gunID, int count)
    {
        PlayerPrefs.SetInt(gunID + "_LockCount", count);
    }

    void CheckWeaponLock()
    {
        int lockCount = GetWeaponLockCount(weapons[selectedWeaponIndex].gunID);

        if (lockCount > 0)
        {
            lockImage.gameObject.SetActive(true);
            styleButton.SetActive(false);
            lockCountText.gameObject.SetActive(true);
            lockCountText.text = lockCount.ToString();
        }
        else
        {
            lockImage.gameObject.SetActive(false);
            styleButton.SetActive(true);
            lockCountText.gameObject.SetActive(false);
        }
    }
    public void OnLockButtonClicked()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Ýnternet baðlantýsý yok. Reklam yüklenemedi.");
            noInternetPanel.SetActive(true);
            NoInterNetAnim(noInternetPanel);
            return;
        }
        // internet varsa
        // hazýrda reklam var mý
        if (adsManager.LoadRewardedAddIsNull())
        {
            adsManager.LoadRewardedAd();
        }
        adsManager.ShowRewardedAd();


    }

    public void OnAdWatched()
    {
        string gunID = weapons[selectedWeaponIndex].gunID;
        int lockCount = GetWeaponLockCount(gunID);

        if (lockCount > 0)
        {
            lockCount--;
            SetWeaponLockCount(gunID, lockCount);

            if (lockCount == 0)
            {
                lockImage.gameObject.SetActive(false);
                styleButton.SetActive(true);
                lockCountText.gameObject.SetActive(false);
            }
            else
            {
                lockCountText.text = lockCount.ToString();
            }
        }
    }
    public void NoInterNetAnim(GameObject obj)
    {
        noInternetPanel.SetActive(true);
        Vector3 originalScale = obj.transform.localScale;
        Vector3 targetScale = originalScale * 1.3f;

        // Önce butonu büyüt
        LeanTween.scale(obj, targetScale, 0.3f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(obj, originalScale, 0.3f)
                              .setEase(LeanTweenType.easeInOutQuad)
                              .setOnComplete(() =>
                              {
                                  // 1 saniye sonra paneli kapat
                                  LeanTween.delayedCall(.5f, () =>
                                  {
                                      noInternetPanel.SetActive(false);
                                  });
                              });
                 });
    }
}