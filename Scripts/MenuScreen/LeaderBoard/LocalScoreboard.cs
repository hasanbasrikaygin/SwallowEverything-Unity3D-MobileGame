using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;

public class LocalScoreboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> nameTexts;
    [SerializeField] private List<TextMeshProUGUI> scoreTexts;

    private const int MaxEntries = 10; // Max 10 entries
    void Start()
    {

        DisplayScores();
    }



    public void DisplayScores()
    {
        // Display scores in UI
        for (int i = 0; i < MaxEntries; i++)
        {
            string playerName = PlayerPrefs.GetString("user" + i, "Unknown");
            int playerScore = PlayerPrefs.GetInt("score" + i, 3600); // Örnek olarak 0 deðeri
            nameTexts[i].text = playerName;
            scoreTexts[i].text = FormatTime(playerScore);
        }
    }
    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}
