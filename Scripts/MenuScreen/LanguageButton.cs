using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private string languageFileName;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetLanguage);
    }

    private void SetLanguage()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.LoadLocalizedText(languageFileName);
            LocalizationManager.Instance.SetSelectedLanguage(languageFileName); // Seçili dili kaydet
        }
        else
        {
            Debug.LogError("LocalizationManager instance is not set.");
        }
    }
}
