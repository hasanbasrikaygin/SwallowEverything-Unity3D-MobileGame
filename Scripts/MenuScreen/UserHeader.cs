using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserHeader : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] TMP_Text userLevel;
    [SerializeField] TMP_Text userName;

    private const string PlayerNameKey = "UserName";
    private const string PlayerKillsKey = "PlayerKills";
    private const int KillsPerLevel = 15; // Level baþýna gerekli kill sayýsý

    void Start()
    {
        // Slider'ýn min ve max deðerlerini kontrol et
        if (loadingSlider != null)
        {
            loadingSlider.minValue = 0;
            loadingSlider.maxValue = 1;
        }
        else
        {
            Debug.LogError("LoadingSlider is not assigned!");
            return;
        }

        // PlayerPrefs'den oyuncu ismini al ve kullanýcý adý olarak ayarla
        UpdateUserName();

        // PlayerPrefs'den kill sayýsýný al
        int totalKills = PlayerPrefs.GetInt(PlayerKillsKey, 0);
        UpdateLevelAndProgress(totalKills);
    }


    void UpdateLevelAndProgress(int totalKills)
    {
        // Level'ý hesapla
        int level = totalKills / KillsPerLevel;
        userLevel.text = level.ToString();

        // Slider deðerini hesapla
        int killsInCurrentLevel = totalKills % KillsPerLevel;
        float sliderValue = (float)killsInCurrentLevel / KillsPerLevel;

        // Slider'ý güncelle
        loadingSlider.value = sliderValue;
    }

    public void UpdateUserName()
    {
        // PlayerPrefs'den oyuncu ismini al ve kullanýcý adý olarak ayarla
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            userName.text = PlayerPrefs.GetString(PlayerNameKey);
        }
        else
        {
            userName.text = "Player"; // Default isim
        }
    }
}
