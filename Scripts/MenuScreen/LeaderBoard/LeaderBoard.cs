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
                scores[i].text = FormatTime(msg[i].Score);  // Timer de�erini formatla ve g�ster
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
        // �nce oyuncunun giri�ini s�f�rlay�n
        LeaderboardCreator.ResetPlayer(() =>
        {
            // Giri� s�f�rlama ba�ar�l�ysa, yeni giri�i y�kleyin
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
        // Global skorlar� Leaderboard API'sinden al
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, (globalScores) =>
        {
            // Local skorlar� al
            List<ScoreEntry> localScores = GetLocalScores();

            // Her bir local skoru kontrol et
            foreach (var localScore in localScores)
            {
                bool scoreExists = false;

                // Global skorlar aras�nda mevcut mu kontrol et
                foreach (var globalScore in globalScores)
                {
                    // Hem isim hem de skor ayn�ysa bu skor zaten mevcut demektir
                    if (globalScore.Username == localScore.Username && globalScore.Score == localScore.Score)
                    {
                        scoreExists = true;
                        break; // Ayn� skor bulundu�unda daha fazla kontrol etmeye gerek yok
                    }
                }

                // E�er bu local skor globalde yoksa, y�kle
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

        // PlayerPrefs'den local skorlar� al ve listeye ekle
        for (int i = 0; i < 10; i++)
        {
            string playerName = PlayerPrefs.GetString("user" + i, "Unknown");
            int playerScore = PlayerPrefs.GetInt("score" + i, 3600);

            // E�er skor s�f�r de�ilse, listeye ekle
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
