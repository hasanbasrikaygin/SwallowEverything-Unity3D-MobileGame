using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    [SerializeField] private string playGameScene;
    [SerializeField] private string outfitsScene;
    [SerializeField] private string gunSkins;
    [SerializeField] Button outfitsSceneButton;
    [SerializeField] Button weaponDesignsSceneButton;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject homePanel;
    [SerializeField] Image exitImage;
    [SerializeField] Image exitImageBg;
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] Button playerNameSaveButton;
    [SerializeField] GameObject warningPanel;
    [SerializeField] TMP_Text warningText;
    [SerializeField] GameObject successPanel;
    [SerializeField] TMP_Text successText;
    [SerializeField] GameObject loadingUi;
    [SerializeField] GameObject languageCanvas;
    [SerializeField] GameObject globalScoreboardPanel;
    [SerializeField] GameObject localScoreBoardPanel;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private UserHeader userHeader;
    private Dictionary<int, float> sceneLoadTimes = new Dictionary<int, float>()
    {
        { 0, 1f }, // Scene 1 should take at least 2 seconds
        { 1, 1f }, // Scene 1 should take at least 2 seconds
        { 3, 1f }, // Scene 1 should take at least 2 seconds
        { 2, 3.5f }, // Scene 2 should take at least 3 seconds
        // Add more scenes and their minimum load times as needed
    };
    private void Awake()
    {
        Debug.Log(gameObject.name + " menu Controller");
        settingsCanvas.SetActive(false);
        globalScoreboardPanel.SetActive(false);
        localScoreBoardPanel.SetActive(false);
        languageCanvas.SetActive(false);
        exitImage.enabled = false;
        exitImageBg.enabled = false;
        settingsPanel.SetActive(false);
        warningPanel.SetActive(false);
        successPanel.SetActive(false); 
        // PlayerPrefs'ten oyuncu ismini al ve InputField'e ayarla
        if (PlayerPrefs.HasKey("UserName"))
        {
            playerNameInput.text = PlayerPrefs.GetString("UserName");
        }
        else
        {
            PlayerPrefs.SetString("UserName", "Unknown");
            playerNameInput.text = "Unknown";
        }

        // Save butonuna týklama olayýný baðla
        playerNameSaveButton.onClick.AddListener(SavePlayerName);

    }
    public void LoadSceneById(int levelId)
    {
        loadingUi.SetActive(true);
        StartCoroutine(LoadingScreenAnim(levelId));
    }

    IEnumerator LoadingScreenAnim(int sceneIndex)
    {
        float minLoadTime = sceneLoadTimes.ContainsKey(sceneIndex) ? sceneLoadTimes[sceneIndex] : 1f; // Default minimum load time
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, progressValue, Time.deltaTime * 2f);

            elapsedTime += Time.deltaTime;
            if (operation.progress >= 0.9f && elapsedTime >= minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        exitImage.enabled = true;
        exitImageBg.enabled = true;
    }
    public void ShowHomePanel()
    {
        homePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void CloesExitImage()
    {
        exitImage.enabled = false;
        exitImageBg.enabled = false;
    }

    private void SavePlayerName()
    {
        // InputField'den oyuncu adýný alýr ve baþtaki/sondaki boþluklarý kaldýrýr
        string playerName = playerNameInput.text.Trim();

        // Eðer oyuncu adý boþsa hata mesajý yazar ve metottan çýkar
        if (string.IsNullOrEmpty(playerName))
        {
            ShowWarning("Player name cannot be empty.");
            return;
        }

        // Oyuncu adýnýn uzunluðunu kontrol et (maksimum 15 karakter)
        if (playerName.Length > 15)
        {
            ShowWarning("Player name cannot be longer than 15 characters.");
            return;
        }

        // Oyuncu adýný PlayerPrefs'e kaydeder
        PlayerPrefs.SetString("UserName", playerName);
        PlayerPrefs.Save(); // Deðiþiklikleri kalýcý hale getirir

        // Oyuncu adýnýn baþarýyla kaydedildiðini konsola yazar
        Debug.Log("Name saved: " + playerName);

        // Baþarý panelini göster
        ShowSuccess("Name saved successfully: " + playerName);
        if (userHeader != null)
        {
            userHeader.UpdateUserName();
        }
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
        Invoke("HideWarning", 1.2f); // 3 saniye sonra uyarý panelini kapat
    }

    private void HideWarning()
    {
        warningPanel.SetActive(false);
    }

    private void ShowSuccess(string message)
    {
        successText.text = message;
        successPanel.SetActive(true);
        Invoke("HideSuccess", 1f); // 3 saniye sonra baþarý panelini kapat
    }

    private void HideSuccess()
    {
        successPanel.SetActive(false);
    }
}
