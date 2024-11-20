using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = " . . . ";

    public event Action OnLanguageChanged;
    private bool isGameLoaded = false;
    [SerializeField] private TextAsset[] languageFiles;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;

    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private TMP_FontAsset arabicFont;
    [SerializeField] private TMP_FontAsset chineseFont;
    [SerializeField] private TMP_FontAsset englishFont;
    [SerializeField] private TMP_FontAsset frenchFont;
    [SerializeField] private TMP_FontAsset germanFont;
    [SerializeField] private TMP_FontAsset hindiFont;
    [SerializeField] private TMP_FontAsset italianFont;
    [SerializeField] private TMP_FontAsset japaneseFont;
    [SerializeField] private TMP_FontAsset koreanFont;
    [SerializeField] private TMP_FontAsset portugueseFont;
    [SerializeField] private TMP_FontAsset russianFont;
    [SerializeField] private TMP_FontAsset spanishFont;
    [SerializeField] private TMP_FontAsset turkishFont;

    private const string LanguagePrefKey = "selectedLanguage";

    private Dictionary<string, TMP_FontAsset> languageFontMap;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLanguageFontMap();
            LoadSelectedLanguage(); // Oyunun ba�lang�c�nda dili y�kle
            RegisterSceneChangeEvent();
            
        }
        else
        {
            Destroy(gameObject);
        }

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

    }

    private void InitializeLanguageFontMap()
    {
        languageFontMap = new Dictionary<string, TMP_FontAsset>
        {
            { "Arabic", arabicFont },
            { "Chinese", chineseFont },
            { "English", englishFont },
            { "French", frenchFont },
            { "German", germanFont },
            { "Hindi", hindiFont },
            { "Italian", italianFont },
            { "Japanese", japaneseFont },
            { "Korean", koreanFont },
            { "Portuguese", portugueseFont },
            { "Russian", russianFont },
            { "Spanish", spanishFont },
            { "Turkish", turkishFont },
            // Varsay�lan font
            { "Default", defaultFont }
        };
    }

    public void LoadSelectedLanguage()
    {
        string selectedLanguage = PlayerPrefs.GetString(LanguagePrefKey, "English");
        LoadLocalizedText(selectedLanguage);
    }

    public void LoadLocalizedText(string fileName)
    {
        StartCoroutine(LoadLocalizedTextCoroutine(fileName));
    }

    private IEnumerator LoadLocalizedTextCoroutine(string fileName)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f); // Slider h�z�n� d���rmek i�in bekleme s�resi

        TextAsset file = Array.Find(languageFiles, item => item.name.Equals(fileName, StringComparison.OrdinalIgnoreCase));

        if (file == null)
        {
            Debug.LogError("Localization file not found: " + fileName);
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }
            yield break;
        }

        localizedText = new Dictionary<string, string>();
        string dataAsJson = file.text;

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            if (!localizedText.ContainsKey(loadedData.items[i].key))
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            else
            {
                Debug.LogWarning("Duplicate key found and ignored: " + loadedData.items[i].key);
            }

            if (loadingSlider != null)
            {
                loadingSlider.value = (float)(i + 1) / loadedData.items.Length;
            }

            yield return new WaitForSeconds(0.02f); // Slider g�ncellemesi i�in daha s�k bekleme s�resi
        }

        isReady = true;
        Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");

        yield return new WaitForSeconds(1f); // Sahne ge�i�inden �nce ek bir bekleme s�resi

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        // Dil dosyas� y�klendikten sonra uygun fontu se�
        SetFontForLanguage(fileName);

        OnLanguageChanged?.Invoke();
    }

    private void SetFontForLanguage(string languageFileName)
    {
        TMP_FontAsset selectedFont = defaultFont; // Varsay�lan font

        if (languageFontMap.TryGetValue(languageFileName, out TMP_FontAsset font))
        {
            selectedFont = font;
            //Debug.Log("Selected font for " + languageFileName + ": " + selectedFont.name);
        }
        else
        {
            Debug.LogWarning("Font not found for language: " + languageFileName + ". Using default font.");
        }

        // T�m TMP_Text bile�enlerine se�ilen fontu ata
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            text.font = selectedFont;
            text.ForceMeshUpdate(); // Bu sat�r, TMP_Text bile�enlerinin mesh'ini g�nceller ve yeni fontu uygular
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string selectedLanguage = PlayerPrefs.GetString(LanguagePrefKey, "English");
        LoadLocalizedText(selectedLanguage);
        SetFontForLanguage(selectedLanguage);
    }

    private void RegisterSceneChangeEvent()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public string GetLocalizedValue(string key)
    {
        if (!isReady)
        {
            Debug.LogWarning("LocalizationManager is not ready. Please load a localization file first.");
            return missingTextString;
        }

        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public void SetSelectedLanguage(string languageFileName)
    {
        PlayerPrefs.SetString(LanguagePrefKey, languageFileName);
        PlayerPrefs.Save();
        LoadLocalizedText(languageFileName); // Yeni sahneye ge�i� yapmadan �nce dil dosyas�n� y�kleyin
    }

    [Serializable]
    private class LocalizationData
    {
        public LocalizationItem[] items;
    }

    [Serializable]
    private class LocalizationItem
    {
        public string key;
        public string value;
    }
}
