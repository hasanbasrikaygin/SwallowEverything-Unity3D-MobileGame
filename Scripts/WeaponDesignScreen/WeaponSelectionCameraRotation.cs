using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmoothCameraTransition : MonoBehaviour
{
    public Button transitionButton;
    public Image transitionImage;
    public Transform target1;
    public Transform target2;
    public Transform target;
    public Transform intermediatePoint; // Ara nokta
    public Canvas colorCanvas;
    public GameObject colorWeaponCanvas;
    public GameObject weaponNameCanvas;
    public float transitionDuration = 1f; // Geçiş süresi
    private bool isTransitioning = false; // Geçiş durumunu izleme
    public WeaponChanger weaponChanger;
    private Transform currentTarget; // Mevcut hedef nokta
    public TextMeshProUGUI buttonNameChanger;

    private void Start()
    {
        transitionButton.onClick.AddListener(TransitionCamera);

        // Başlangıçta ilk hedef noktayı ayarla
        currentTarget = target1;
       
        // Başlangıçta canvas'leri doğru şekilde ayarla
        colorCanvas.gameObject.SetActive(true);
        colorWeaponCanvas.gameObject.SetActive(false);
    }

    private void TransitionCamera()
    {
        if (!isTransitioning)
        {
            // Mevcut hedef noktayı değiştir
            buttonNameChanger.enabled = false;
            colorCanvas.gameObject.SetActive(false);
            weaponNameCanvas.gameObject.SetActive(false);
            transitionButton.enabled = false;
            transitionImage.enabled = false;

            currentTarget = (currentTarget == target1) ? target2 : target1;
             if (currentTarget == target2)
            {
                ToggleAllWeapons();
            }
            StartCoroutine(TransitionCoroutine());
        }
    }

    public void ToggleAllWeapons()
    {
        if (weaponChanger != null)
        {
            weaponChanger.ToggleAllWeapons();
        }
    }
    public void ShowAllWeapons()
    {
        if (weaponChanger != null)
        {
            weaponChanger.ShowAllWeapons();
        }
    }
    private IEnumerator TransitionCoroutine()
    {
        isTransitioning = true;

        // Canvas'leri geçici olarak devre dışı bırak
        colorCanvas.gameObject.SetActive(false);
        colorWeaponCanvas.gameObject.SetActive(false);

        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        // İlk önce ara noktaya git
        while (elapsedTime < transitionDuration)
        {
         
            elapsedTime += Time.deltaTime;

            // Geçiş süresine göre interpolasyon yap
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            transform.position = Vector3.Lerp(startPosition, intermediatePoint.position, t);
            transform.rotation = Quaternion.Lerp(startRotation, target.rotation, t);
            yield return null;
        }
        
        // Sonra hedef noktaya git
        elapsedTime = 0f;
        startPosition = transform.position;
        startRotation = transform.rotation;

        while (elapsedTime < transitionDuration)
        {
          
            elapsedTime += Time.deltaTime;

            // Geçiş süresine göre interpolasyon yap
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            transform.position = Vector3.Lerp(startPosition, currentTarget.position, t);
            transform.rotation = Quaternion.Lerp(startRotation, currentTarget.rotation, t);
           
            yield return null;
        }

        isTransitioning = false;

        // Kamera hareketi tamamlandığında canvas'leri aç
        if (currentTarget == target1)
        {
            colorCanvas.gameObject.SetActive(true);
            ShowAllWeapons();
            buttonNameChanger.enabled = true;
            transitionButton.enabled = true;
            transitionImage.enabled = true;
            weaponNameCanvas.gameObject.SetActive(true);

        }
        else if (currentTarget == target2)
        {
            colorWeaponCanvas.gameObject.SetActive(true);
            buttonNameChanger.enabled = true;
            transitionButton.enabled = true;
            transitionImage.enabled = true;
            weaponNameCanvas.gameObject.SetActive(true);
        }
    }
}
