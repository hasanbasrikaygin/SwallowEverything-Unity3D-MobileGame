using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullan�yorsan�z, TMP InputField i�in gerekli

public class SensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInputField; // TextMeshPro InputField kullan�yorsan�z
    // public InputField sensitivityInputField; // E�er Unity'nin standart InputField'�n� kullan�yorsan�z

    private float sensitivity;
    private const string SensitivityPrefKey = "Sensitivity";

    private void Start()
    {
        // Sensitivity de�erini PlayerPrefs'den al, e�er yoksa varsay�lan de�er 1.0 olarak ayarla
        sensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, 3f);

        // Slider ve Input Field'� ba�latma
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
        // Sensitivity de�erini g�ncelle
        UpdateSensitivity(sensitivity);
    }

    private void OnInputFieldEndEdit(string value)
    {
        if (int.TryParse(value, out int intValue))
        {
            float newSensitivity = intValue / 100f; // 100'e b�lerek iki ondal�k basama�a �evir
            sensitivity = Mathf.Clamp(newSensitivity, 0f, 8f);
            sensitivitySlider.value = sensitivity;
            sensitivityInputField.text = sensitivity.ToString("F2");
            // Sensitivity de�erini g�ncelle
            UpdateSensitivity(sensitivity);
        }
        else
        {
            // Ge�ersiz bir de�er girilirse mevcut de�eri yeniden yazd�r
            sensitivityInputField.text = sensitivity.ToString("F2");
        }
    }

    private void UpdateSensitivity(float newSensitivity)
    {
        // Sensitivity ayarlar�n� g�ncelle
        // Bu k�s�mda sensitivity de�erini kullanarak gerekli ayarlamalar� yapabilirsiniz.
        //Debug.Log("New Sensitivity: " + newSensitivity);

        // Sensitivity de�erini PlayerPrefs'e kaydet
        PlayerPrefs.SetFloat(SensitivityPrefKey, newSensitivity);
        PlayerPrefs.Save();
    }
}
