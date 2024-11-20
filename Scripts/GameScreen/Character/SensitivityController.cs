using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanýyorsanýz, TMP InputField için gerekli

public class SensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInputField; // TextMeshPro InputField kullanýyorsanýz
    // public InputField sensitivityInputField; // Eðer Unity'nin standart InputField'ýný kullanýyorsanýz

    private float sensitivity;
    private const string SensitivityPrefKey = "Sensitivity";

    private void Start()
    {
        // Sensitivity deðerini PlayerPrefs'den al, eðer yoksa varsayýlan deðer 1.0 olarak ayarla
        sensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, 3f);

        // Slider ve Input Field'ý baþlatma
        sensitivitySlider.minValue = 0f;
        sensitivitySlider.maxValue = 8f;

        sensitivitySlider.value = sensitivity;
        sensitivityInputField.text = sensitivity.ToString("F2");

        // Slider ve Input Field'a event ekleme
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
        sensitivityInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnSliderValueChanged(float value)
    {
        sensitivity = Mathf.Clamp(value, 0f, 8f);
        sensitivityInputField.text = sensitivity.ToString("F2");
        // Sensitivity deðerini güncelle
        UpdateSensitivity(sensitivity);
    }

    private void OnInputFieldEndEdit(string value)
    {
        if (int.TryParse(value, out int intValue))
        {
            float newSensitivity = intValue / 100f; // 100'e bölerek iki ondalýk basamaða çevir
            sensitivity = Mathf.Clamp(newSensitivity, 0f, 8f);
            sensitivitySlider.value = sensitivity;
            sensitivityInputField.text = sensitivity.ToString("F2");
            // Sensitivity deðerini güncelle
            UpdateSensitivity(sensitivity);
        }
        else
        {
            // Geçersiz bir deðer girilirse mevcut deðeri yeniden yazdýr
            sensitivityInputField.text = sensitivity.ToString("F2");
        }
    }

    private void UpdateSensitivity(float newSensitivity)
    {
        // Sensitivity ayarlarýný güncelle
        // Bu kýsýmda sensitivity deðerini kullanarak gerekli ayarlamalarý yapabilirsiniz.
        //Debug.Log("New Sensitivity: " + newSensitivity);

        // Sensitivity deðerini PlayerPrefs'e kaydet
        PlayerPrefs.SetFloat(SensitivityPrefKey, newSensitivity);
        PlayerPrefs.Save();
    }
}
