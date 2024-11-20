using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    private string publicLeaderBoardKey = "d4dbe9a036be59e42fab16d942a44201f362903a18f5533b5a2f81bbb10c9dc9";

    void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection. Cannot upload score.");
            return;
        }
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = FormatTime(msg[i].Score);  // Timer deðerini formatla ve göster
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection. Cannot upload score.");
            return;
        }
        // Önce oyuncunun giriþini sýfýrlayýn
        LeaderboardCreator.ResetPlayer(() =>
        {
            // Giriþ sýfýrlama baþarýlýysa, yeni giriþi yükleyin
            LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, (uploadSuccess) =>
            {
                if (uploadSuccess)
                {
                    GetLeaderboard();
                }
                else
                {
                    Debug.LogError("Failed to upload new entry.");
                }
            });
        }, (error) =>
        {
            Debug.LogError("Error resetting player: " + error);
        });
    }

    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public void CompareAndUploadScores()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("No internet connection. Cannot upload score.");
            return;
        }
        // Global skorlarý Leaderboard API'sinden al
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, (globalScores) =>
        {
            // Local skorlarý al
            List<ScoreEntry> localScores = GetLocalScores();

            // Her bir local skoru kontrol et
            foreach (var localScore in localScores)
            {
                bool scoreExists = false;

                // Global skorlar arasýnda mevcut mu kontrol et
                foreach (var globalScore in globalScores)
                {
                    // Hem isim hem de skor aynýysa bu skor zaten mevcut demektir
                    if (globalScore.Username == localScore.Username && globalScore.Score == localScore.Score)
                    {
                        scoreExists = true;
                        break; // Ayný skor bulunduðunda daha fazla kontrol etmeye gerek yok
                    }
                }

                // Eðer bu local skor globalde yoksa, yükle
                if (!scoreExists)
                {
                    SetLeaderboardEntry(localScore.Username, localScore.Score);
                }
            }
        });
    }

    private List<ScoreEntry> GetLocalScores()
    {
        List<ScoreEntry> localScores = new List<ScoreEntry>();

        // PlayerPrefs'den local skorlarý al ve listeye ekle
        for (int i = 0; i < 10; i++)
        {
            string playerName = PlayerPrefs.GetString("user" + i, "Unknown");
            int playerScore = PlayerPrefs.GetInt("score" + i, 3600);

            // Eðer skor sýfýr deðilse, listeye ekle
            if (playerScore != 0)
            {
                localScores.Add(new ScoreEntry(playerName, playerScore));
            }
        }

        return localScores;
    }

    private struct ScoreEntry
    {
        public string Username;
        public int Score;

        public ScoreEntry(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }
}
