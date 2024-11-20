using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    // Can göstergesi için slider referansý
    public Slider healthSlider;

    void Start()
    {
        // Eðer healthSlider atanmamýþsa hata ver ve fonksiyonu sonlandýr
        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider is not assigned!");
            return;
        }

        // Saðlýk göstergesinin maksimum ve minimum deðerlerini ayarla
        healthSlider.maxValue = PlayerHealth.maxHealth;
        healthSlider.minValue = 0;
    }

    void Update()
    {
        // Eðer healthSlider atanmýþsa HealthBar'ý güncelle
        if (healthSlider != null)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        // Saðlýk çubuðunun doluluk oranýný güncelle
        healthSlider.value = PlayerHealth.currentHealth;
    }
}
