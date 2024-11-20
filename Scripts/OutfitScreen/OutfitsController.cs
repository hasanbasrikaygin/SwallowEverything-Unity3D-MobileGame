using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutfitsController : MonoBehaviour
{
    private int currentClothingIndex;

    public Button rightButton;
    public Button leftButton;
    public Button backButton;

    public GameObject noInternetPanel;


    [SerializeField] SkinnedMeshRenderer bodyRenderer;
    [SerializeField] SkinnedMeshRenderer legsRenderer;
    [SerializeField] SkinnedMeshRenderer feetRenderer;
    [SerializeField] SkinnedMeshRenderer headRenderer;

    public List<ClothingSet> bodyClothes;
    public List<ClothingSet> legsClothes;
    public List<ClothingSet> feetClothes;
    public List<ClothingSet> headClothes;

    public TextMeshProUGUI adventurerText;
    public TextMeshProUGUI kingText;
    public TextMeshProUGUI casualText;
    public TextMeshProUGUI casual2Text;
    public TextMeshProUGUI space_suitText;
    public TextMeshProUGUI suitText;
    public TextMeshProUGUI farmerText;
    public TextMeshProUGUI punkText;
    public TextMeshProUGUI beachText;
    public TextMeshProUGUI swatText;
    public TextMeshProUGUI workerText;

    public GameObject lockIcon; // Lock icon to indicate locked clothing
    public TextMeshProUGUI unlockAttemptsText; // Text to show remaining unlock attempts

    private Dictionary<string, TextMeshProUGUI> clothingTexts;

    public OutFitsAdsManager adsManager;

    private void Awake()
    {
        clothingTexts = new Dictionary<string, TextMeshProUGUI>
        {
            { "adventurer", adventurerText },
            { "king", kingText },
            { "casual", casualText },
            { "casual2", casual2Text },
            { "space_suit", space_suitText },
            { "suit", suitText },
            { "farmer", farmerText },
            { "punk", punkText },
            { "beach", beachText },
            { "swat", swatText },
            { "worker", workerText }
        };

        // Initialize clothing lock states
        InitializeClothingLocks();
    }

    private void Start()
    {
        currentClothingIndex = PlayerPrefs.GetInt("SelectedClothingIndex", 0);
        ApplyClothingIndex();
        noInternetPanel.SetActive(false);
        rightButton.onClick.AddListener(NextClothing);
        leftButton.onClick.AddListener(PreviousClothing);
        backButton.onClick.AddListener(ReturnHome);

    }

    private void InitializeClothingLocks()
    {
        if (!PlayerPrefs.HasKey("ClothingLocksInitialized"))
        {
            // List of clothing that should be unlocked by default
            List<string> defaultUnlockedClothes = new List<string>
            {
                "adventurer",
                "casual"
                // Add other default unlocked clothes here
            };

            foreach (var clothing in bodyClothes)
            {
                string clothingName = clothing.name;
                if (defaultUnlockedClothes.Contains(clothingName.ToLower()))
                {
                    PlayerPrefs.SetInt(clothingName + "_locked", 0); // Unlock the clothing
                }
                else
                {
                    PlayerPrefs.SetInt(clothingName + "_locked", 1); // Lock the clothing
                    PlayerPrefs.SetInt(clothingName + "_unlockAttempts", 3); // Set initial unlock attempts
                }
            }

            PlayerPrefs.SetInt("ClothingLocksInitialized", 1);
        }
    }

    void NextClothing()
    {
        if (bodyClothes != null && bodyClothes.Count > 0)
        {
            currentClothingIndex = (currentClothingIndex + 1) % bodyClothes.Count;
            ApplyClothingIndex();
        }
    }

    void PreviousClothing()
    {
        if (bodyClothes != null && bodyClothes.Count > 0)
        {
            currentClothingIndex = (currentClothingIndex - 1 + bodyClothes.Count) % bodyClothes.Count;
            ApplyClothingIndex();
        }
    }

    private void ApplyClothingIndex()
    {
        ApplyClothing(bodyRenderer, bodyClothes[currentClothingIndex]);
        ApplyClothing(legsRenderer, legsClothes[currentClothingIndex]);
        ApplyClothing(feetRenderer, feetClothes[currentClothingIndex]);
        ApplyClothing(headRenderer, headClothes[currentClothingIndex]);

        UpdateClothingName(bodyClothes[currentClothingIndex].name);

        // Check if the current clothing is locked and update the lock icon
        if (IsClothingLocked(currentClothingIndex))
        {
            lockIcon.SetActive(true);
            int unlockAttempts = PlayerPrefs.GetInt(bodyClothes[currentClothingIndex].name + "_unlockAttempts", 3);
            unlockAttemptsText.text = unlockAttempts.ToString();
            unlockAttemptsText.gameObject.SetActive(true);
        }
        else
        {
            lockIcon.SetActive(false);
            unlockAttemptsText.gameObject.SetActive(false);
            PlayerPrefs.SetInt("SelectedClothingIndex", currentClothingIndex);
            PlayerPrefs.Save();
        }
    }

    private void ApplyClothing(SkinnedMeshRenderer renderer, ClothingSet clothingSet)
    {
        Debug.Log($"Applying clothing: {clothingSet.name}");
        renderer.sharedMesh = clothingSet.mesh;
        renderer.materials = clothingSet.materials;
    }

    private void UpdateClothingName(string outfitName)
    {
        foreach (var text in clothingTexts.Values)
        {
            text.gameObject.SetActive(false);
        }

        if (clothingTexts.TryGetValue(outfitName.ToLower(), out var tmpText))
        {
            tmpText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Unknown clothing name: " + outfitName);
        }
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("Home");
    }

    private bool IsClothingLocked(int index)
    {
        string clothingName = bodyClothes[index].name;
        int lockedValue = PlayerPrefs.GetInt(clothingName + "_locked", 1); // 1 means locked, 0 means unlocked
        Debug.Log($"Clothing {clothingName} is {(lockedValue == 1 ? "locked" : "unlocked")}");
        return lockedValue == 1;
    }

    public void UnlockClothing(int index)
    {
        string clothingName = bodyClothes[index].name;
        PlayerPrefs.SetInt(clothingName + "_locked", 0); // Unlock the clothing
        PlayerPrefs.SetInt("SelectedClothingIndex", currentClothingIndex); // Save the current selection
        PlayerPrefs.Save(); // Ensure the changes are saved
    }

    public void OnLockIconClicked()
    {
        // 1) internet var mý ?
            // 2) var : hazýrda reklam  var mý ?
                // 3) evet  :  reklamý göster - ödülü ver
                // 4) hayýr :  reklamý yükle - reklamý göster
            // 5) yok : internet yok panelini göster

        Debug.Log("Lock icon clicked");
        // Geçerli indeksteki silahýn kilidi var mý 
        if (IsClothingLocked(currentClothingIndex))
        {
            // internet yoksa
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Ýnternet baðlantýsý yok. Reklam yüklenemedi.");
                noInternetPanel.SetActive(true);
                NoInterNetAnim(noInternetPanel);
                return;
            }
            // internet varsa
            // hazýrda reklam var mý
            if(adsManager.LoadRewardedAddIsNull())
            {
                adsManager.LoadRewardedAd();
            }
            adsManager.ShowRewardedAd();
            
        }
    }

    public void OnAdWatched()
    {
        Debug.Log("Unlocking clothing...");
        int unlockAttempts = PlayerPrefs.GetInt(bodyClothes[currentClothingIndex].name + "_unlockAttempts", 3);
        unlockAttempts--;

        if (unlockAttempts <= 0)
        {
            UnlockClothing(currentClothingIndex);
            lockIcon.SetActive(false);
            unlockAttemptsText.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt(bodyClothes[currentClothingIndex].name + "_unlockAttempts", unlockAttempts);
            unlockAttemptsText.text = unlockAttempts.ToString();
        }

        PlayerPrefs.Save(); // Ensure the changes are saved
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
