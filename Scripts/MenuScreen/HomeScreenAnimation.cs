using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenAnimation : MonoBehaviour
{
    [SerializeField] LeanTweenType showPanelLeanTweenType;
    [SerializeField] LeanTweenType closePanelLeanTweenType;

    [SerializeField] private float buttonScaleFactor = 1.2f;  // Büyütme oraný
    [SerializeField] private float buttonAnimationDuration = 0.1f;  // Animasyon süresi

    [Header("Home")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject leftPanel;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject userHeader;

    private Vector3 playButtonStartPos;
    private Vector3 scoreboardButtonStartPos;
    private Vector3 settingsButtonStartPos;
    private Vector3 userHeaderStartPos;

    [Header("LeaderBoard")]
    [SerializeField] GameObject localLeaderBoardCanvas;
    [SerializeField] GameObject globalLeaderBoardCanvas;
    [SerializeField] GameObject localButton;
    [SerializeField] GameObject globalButton;
    [SerializeField] GameObject localExitButton;
    [SerializeField] GameObject globalExitButton;

    [Header("Settings")]
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject nameConfirm;
    [SerializeField] GameObject languageButton;
    [SerializeField] GameObject languagePanel;
    [SerializeField] GameObject languagesButton;
    [SerializeField] GameObject warning;
    [SerializeField] GameObject succes;
    [SerializeField] GameObject exitImage;
    [SerializeField] GameObject exitDialog;
    [SerializeField] GameObject exitDialogYes;
    [SerializeField] GameObject exitDialogNo;
    [SerializeField] GameObject settingsUi;
    [SerializeField] GameObject settingsBackground;
    [SerializeField] GameObject menuCanvas;

    [Header("Transition")]
    [SerializeField] GameObject transitionCanvas;
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject transitionBackground;
    [SerializeField] GameObject enemyRobotImage;
    [SerializeField] GameObject currentPlayerImage;
    [SerializeField] GameObject playerPanel;
    [SerializeField] GameObject robotPanel;
    [SerializeField] List<Sprite> playerCostumeSprites;
    [SerializeField] private GameObject menuControllerObject;
    public LeanTweenType characterType;
    public LeanTweenType backgroundType;
    float currentCostume;
    public AudioSource whooshAudioSource;
    public AudioClip whooshBackground;
    public AudioClip whooshCharacter;
    private MenuController menuController;
    private float animationDuration = 0.5f;
    private float delayBetweenAnimations = 0.2f;
    void Awake()
    {
        // Baþlangýç konumlarýný kaydet
        playButtonStartPos = playButton.transform.localPosition;
        scoreboardButtonStartPos = leftPanel.transform.localPosition;

        settingsButtonStartPos = settingsButton.transform.localPosition;
        userHeaderStartPos = userHeader.transform.localPosition;


        userHeader.transform.localPosition = new Vector3(-Screen.width- 500f, userHeaderStartPos.y, userHeaderStartPos.z);
        leftPanel.transform.localPosition = new Vector3(-Screen.width-500f, scoreboardButtonStartPos.y, scoreboardButtonStartPos.z);


        settingsButton.transform.localPosition = new Vector3(Screen.width+ 500f, settingsButtonStartPos.y, settingsButtonStartPos.z);
        playButton.transform.localPosition = new Vector3(Screen.width+ 500f, playButtonStartPos.y, playButtonStartPos.z);
    }

    void Start()
    {

        if (menuControllerObject != null)
        {
            menuController = menuControllerObject.GetComponent<MenuController>();
        }
        StartCoroutine(MenuUi());
    }
    IEnumerator MenuUi()
    {
        yield return new WaitForSeconds(.3f);

        LeanTween.moveLocal(userHeader, userHeaderStartPos, animationDuration).setEase(LeanTweenType.easeInOutQuad).setDelay(0);
        LeanTween.moveLocal(leftPanel, scoreboardButtonStartPos, animationDuration).setEase(LeanTweenType.easeInOutQuad).setDelay(delayBetweenAnimations);
        LeanTween.moveLocal(settingsButton, settingsButtonStartPos, animationDuration).setEase(LeanTweenType.easeInOutQuad).setDelay(0);
        LeanTween.moveLocal(playButton, playButtonStartPos, animationDuration).setEase(LeanTweenType.easeInOutQuad).setDelay(delayBetweenAnimations);
    }
    public void PlayTransitionAnimations()
    {
        uiCanvas.SetActive(false);
        BackgroundSoundController.Instance.StopBackgroundMusic();
        transitionCanvas.SetActive(true);   
        currentCostume = PlayerPrefs.GetInt("SelectedClothingIndex", 0);
        currentPlayerImage.GetComponent<Image>().sprite = playerCostumeSprites[(int)currentCostume];
        // Transition background animation
        whooshAudioSource.PlayOneShot(whooshBackground);
        LeanTween.moveLocalY(transitionBackground, 0f, 1f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                whooshAudioSource.PlayOneShot(whooshCharacter);
                // Enemy robot image animation
                LeanTween.moveLocalY(enemyRobotImage, robotPanel.transform.localPosition.y, 1.2f)
                    .setEase(LeanTweenType.easeInOutElastic)
                    .setOnComplete(() =>
                    {
                        whooshAudioSource.PlayOneShot(whooshCharacter);
                        // Current player image animation
                        LeanTween.moveLocalY(currentPlayerImage, playerPanel.transform.localPosition.y, 1.2f)
                            .setEase(LeanTweenType.easeInOutElastic).setOnComplete(() =>
                            {
                                menuController.LoadSceneById(3);
                            });
                    });
            });
    }
    
    public void OpenCanvas(GameObject openGameObject)
    {
        openGameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        openGameObject.SetActive(true);
        LeanTween.scale(openGameObject, new Vector3(.8f, .8f, .8f), .2f)
                 .setDelay(.1f)
                 .setEase(showPanelLeanTweenType);
    }

    public void CloseCanvas(GameObject closeGameObject)
    {
        LeanTween.scale(closeGameObject, new Vector3(0f, 0f, 0f), .2f)
                 .setDelay(.1f)
                 .setEase(closePanelLeanTweenType)
                 .setOnComplete(() => closeGameObject.SetActive(false));
    }

    public void OnButtonClick(GameObject button)
    {
        
        Vector3 originalScale = button.transform.localScale;
        Vector3 targetScale = originalScale * buttonScaleFactor;

        // Önce butonu büyüt
        LeanTween.scale(button, targetScale, buttonAnimationDuration)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(button, originalScale, buttonAnimationDuration)
                              .setEase(LeanTweenType.easeInOutQuad);
                 });
    }    
    public void OnButtonClickLeaderBoard(GameObject button)
    {
        
        Vector3 originalScale = button.transform.localScale;
        Vector3 targetScale = originalScale * buttonScaleFactor;

        // Önce butonu büyüt
        LeanTween.scale(button, targetScale, buttonAnimationDuration)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(button, originalScale, buttonAnimationDuration)
                              .setEase(LeanTweenType.easeInOutQuad);
                 });
    }   
    public void OnButtonClickLanguage(GameObject button )
    {
        
        Vector3 originalScale = button.transform.localScale;
        Vector3 targetScale = originalScale * buttonScaleFactor;

        // Önce butonu büyüt
        LeanTween.scale(button, targetScale, buttonAnimationDuration)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(button, originalScale, buttonAnimationDuration)
                              .setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                              {
                                  settingsUi.SetActive(false);
                                  settingsBackground.SetActive(false);
                                  OpenCanvas(languagePanel);
                                  
                              });
                 });
    }
    public void UpdateScale(GameObject panel)
    {
        panel.transform.localScale = new Vector3(.8f,.8f,.8f);
    }

}
