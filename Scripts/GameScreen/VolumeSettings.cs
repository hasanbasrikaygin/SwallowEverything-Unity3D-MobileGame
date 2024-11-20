using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;



public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;   
    [SerializeField] private Slider sfxButtonSlider;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsExitPanel;
    [SerializeField] private GameObject settingsBackgroundPanel;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;
    [SerializeField] private LeanTweenType leanTweenType;

    // Start is called before the first frame update
    private void Start()
    {
        settingsPanel.SetActive(false);
        settingsExitPanel.SetActive(false);
        settingsBackgroundPanel.SetActive(false);
        if (PlayerPrefs.HasKey("sfxGameVolume"))
        {
            LoadSfxVolume();
        }
        else
        {
            SetSfxVolume();
        }   
        if (PlayerPrefs.HasKey("sfxButtonGameVolume"))
        {
            LoadSfxButtonVolume();
        }
        else
        {
            SetSfxButtonVolume();
        }
    }
    public void SetSfxVolume()
    {
        float sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("sfxGame", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxGameVolume", sfxVolume);
    }
    private void LoadSfxVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxGameVolume");
        SetSfxVolume();
    } 
    public void SetSfxButtonVolume()
    {
        float sfxButtonVolume = sfxButtonSlider.value;
        audioMixer.SetFloat("sfxButtonGame", Mathf.Log10(sfxButtonVolume) * 20);
        PlayerPrefs.SetFloat("sfxButtonGameVolume", sfxButtonVolume);
    }
    private void LoadSfxButtonVolume()
    {
        sfxButtonSlider.value = PlayerPrefs.GetFloat("sfxButtonGameVolume");
        SetSfxButtonVolume();
    }
    public void ManageEnableSettingsPanel()
    {

        
    }
    public void PausePanelInfo()
    {
        settingsPanel.SetActive(true);
        settingsExitPanel.SetActive(true);
        settingsBackgroundPanel.SetActive(true);
        MovePanelToTargetY(settingsPanel);

    }
    public void PausePanelOutro()
    {
        ClosePanelToTargetY(settingsPanel);
    }



    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float targetYPosition = 100f; // Hedef y pozisyonu
    [SerializeField] private float animationDuration = 0.5f; // Animasyon süresi

    public void MovePanelToTargetY(GameObject obj)
    {

        LeanTween.moveLocalY(obj, targetYPosition, .5f).setEase(leanTweenType);
    }
    public void ClosePanelToTargetY(GameObject obj)
    {

        LeanTween.moveLocalY(obj, 950, .5f).setEase(leanTweenType).setOnComplete(() =>
        {
            settingsPanel.SetActive(false);
            settingsExitPanel.SetActive(false);
            settingsBackgroundPanel.SetActive(false);
            Time.timeScale = 1f;
        });
    }
    public void OnButtonClick(GameObject button)
    {

        Vector3 originalScale = button.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f; ;

        // Önce butonu büyüt
        LeanTween.scale(button, targetScale, .1f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(button, originalScale, .1f)
                              .setEase(LeanTweenType.easeInOutQuad);
                 });
        
    }
    public void RotateButton(GameObject obj)
    {
        LeanTween.rotateAroundLocal(obj, Vector3.forward, 180f, 1f);
    }
}
