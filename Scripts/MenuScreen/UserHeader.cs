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
    private const int KillsPerLevel = 15; // Level ba��na gerekli kill say�s�

    void Start()
    {
        // Slider'�n min ve max de�erlerini kontrol et
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

        // PlayerPrefs'den oyuncu ismini al ve kullan�c� ad� olarak ayarla
        UpdateUserName();

        // PlayerPrefs'den kill say�s�n� al
        int totalKills = PlayerPrefs.GetInt(PlayerKillsKey, 0);
        UpdateLevelAndProgress(totalKills);
    }


    void UpdateLevelAndProgress(int totalKills)
    {
        // Level'� hesapla
        int level = totalKills / KillsPerLevel;
        userLevel.text = level.ToString();

        // Slider de�erini hesapla
        int killsInCurrentLevel = totalKills % KillsPerLevel;
        float sliderValue = (float)killsInCurrentLevel / KillsPerLevel;

        // Slider'� g�ncelle
        loadingSlider.value = sliderValue;
    }

    public void UpdateUserName()
    {
        // PlayerPrefs'den oyuncu ismini al ve kullan�c� ad� olarak ayarla
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
