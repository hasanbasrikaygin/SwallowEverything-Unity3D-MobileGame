using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    // Can g�stergesi i�in slider referans�
    public Slider healthSlider;

    void Start()
    {
        // E�er healthSlider atanmam��sa hata ver ve fonksiyonu sonland�r
        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider is not assigned!");
            return;
        }

        // Sa�l�k g�stergesinin maksimum ve minimum de�erlerini ayarla
        healthSlider.maxValue = PlayerHealth.maxHealth;
        healthSlider.minValue = 0;
    }

    void Update()
    {
        // E�er healthSlider atanm��sa HealthBar'� g�ncelle
        if (healthSlider != null)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        // Sa�l�k �ubu�unun doluluk oran�n� g�ncelle
        healthSlider.value = PlayerHealth.currentHealth;
    }
}
