using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;

    private TMP_Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        if (LocalizationManager.Instance != null)
        {
            UpdateText();
            LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        }
        else
        {
            Debug.LogError("LocalizationManager instance is not set.");
        }
    }

    private void UpdateText()
    {
        if (LocalizationManager.Instance != null)
        {
            textComponent.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
        }
    }
}
