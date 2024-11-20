using UnityEngine;
using TMPro;
using UnityEngine.UI; // Slider'lar için gerekli
using System.Collections;
using static PlayerHealth;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int enemyCount = 5;
    public int houseCount = 70;
    public int uiEnemyCount = 5;
    public int uiHouseCount = 70;

    private float timer;

    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text houseCountText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] TMP_Text destroyedHouseCounterText, killedEnemyCounterText, endGameTimer;

    [SerializeField] private Slider enemyCountSlider; // Düþman sayýsý sliderý
    [SerializeField] private Slider houseCountSlider; // Ev sayýsý sliderý

    [SerializeField] TMP_Text winDestroyedHouseCounterText, winKilledEnemyCounterText, winEndGameTimer;

    [SerializeField] private Slider winEnemyCountSlider; // Düþman sayýsý sliderý
    [SerializeField] private Slider winHouseCountSlider; // Ev sayýsý sliderý
    [SerializeField] private GameObject gameWinCanvas; // Ev sayýsý sliderý

    private int destroyedHouseCounter;
    private int killedEnemyCounter;
    public bool isGameOver = false;
    public bool isGameWin = false;
    public bool timerStarted = false;
    private const int MaxEntries = 10;

    private List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
    private const string PlayerKillsKey = "PlayerKills";
    private void Awake()
    {
        destroyedHouseCounter = 0;
        killedEnemyCounter = 0;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        UpdateEnemyCountUI();
        UpdateHouseCountUI();
        UpdateTimerUI(timerText);

        // Slider'larýn maksimum deðerlerini ayarla
        enemyCountSlider.maxValue = enemyCount;
        houseCountSlider.maxValue = houseCount;
        winEnemyCountSlider.maxValue = enemyCount;
        winHouseCountSlider.maxValue = houseCount;
        StartCoroutine(StartTimerWithDelay(20f));
    }
        private IEnumerator StartTimerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        timerStarted = true;
    }
    private void Update()
    {
        if (!isGameOver && timerStarted)
        {
            timer += Time.deltaTime;
            UpdateTimerUI(timerText);
        }
    }

    public void DecreaseEnemyCount()
    {
        enemyCount = Mathf.Max(0, enemyCount - 1);
        killedEnemyCounter++;
        UpdateEnemyCountUI();
        CheckGameWinCondition();
    }

    public void DecreaseHouseCount()
    {
        houseCount = Mathf.Max(0, houseCount - 1);
        destroyedHouseCounter++;
        UpdateHouseCountUI();
        CheckGameWinCondition();
    }

    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "" + enemyCount;
        }
        if (killedEnemyCounterText != null)
        {
            killedEnemyCounterText.text = killedEnemyCounter + " / " + enemyCount;
        }
    }

    private void UpdateHouseCountUI()
    {
        if (houseCountText != null)
        {
            houseCountText.text = "" + houseCount;
          
        }
        else
        {
            Debug.Log("houseCountText is null");
        }
        if (destroyedHouseCounterText != null)
        {
            destroyedHouseCounterText.text = destroyedHouseCounter + " / " + houseCount;
        }
    }

    private void UpdateTimerUI(TMP_Text textUi)
    {
        if (textUi != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            textUi.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }

    public void UpdateEndTimerUI()
    {
        if (endGameTimer != null)
        {
            endGameTimer.text = GetElapsedTime().ToString();
        }
    }

    public int GetElapsedTime()
    {
        return Mathf.FloorToInt(timer);
    }

    public void UpdateEndGameUI()
    {
        int totalKills = PlayerPrefs.GetInt(PlayerKillsKey, 0);
        totalKills += killedEnemyCounter;
        PlayerPrefs.SetInt(PlayerKillsKey, totalKills);
        StartCoroutine(UpdateSlider(enemyCountSlider, killedEnemyCounter, killedEnemyCounterText, uiEnemyCount));
        StartCoroutine(UpdateSlider(houseCountSlider, destroyedHouseCounter, destroyedHouseCounterText, uiHouseCount));
    }

    public void UpdateWinGameUI()
    {
        int totalKills = PlayerPrefs.GetInt(PlayerKillsKey, 0);
        totalKills += killedEnemyCounter;
        PlayerPrefs.SetInt(PlayerKillsKey, totalKills);
        int elapsedTime = GetElapsedTime();
        string playerName = PlayerPrefs.GetString("UserName", "Unknown");
        Debug.Log(playerName);
        Debug.Log(elapsedTime);
        UpdateLeaderboard(playerName, elapsedTime);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            LeaderBoard leaderBoard = new LeaderBoard();
            if (leaderBoard != null)
            {
                leaderBoard.SetLeaderboardEntry(playerName, elapsedTime);
            }
            else
            {
                Debug.LogError("LeaderBoard nesnesi bulunamadý!");
            }
        }
        StartCoroutine(UpdateSlider(winEnemyCountSlider, killedEnemyCounter, winKilledEnemyCounterText, uiEnemyCount));
        StartCoroutine(UpdateSlider(winHouseCountSlider, destroyedHouseCounter, winDestroyedHouseCounterText, uiHouseCount));
        UpdateTimerUI(winEndGameTimer); // Zamaný Game Win ekranýnda da göster
    }

    private IEnumerator UpdateSlider(Slider slider, int targetValue, TMP_Text counterText, int maxValue)
    {
        float startValue = slider.value;
        float duration = 2f; // Dolum süresi, bunu ayarlayarak dolum hýzýný deðiþtirebilirsiniz
        float elapsedTime = 0f;
        LeanTween.scaleY(slider.gameObject,  1.1f, 0.5f).setEase(LeanTweenType.easeOutBack).setLoopPingPong(1);
        LeanTween.color(slider.fillRect.gameObject, Color.green, 1f).setEase(LeanTweenType.easeInOutQuad);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            slider.value = newValue;
            counterText.text = Mathf.FloorToInt(newValue) + " / " + maxValue;
            yield return null; // Her karede güncelle
        }
        slider.value = targetValue;
        counterText.text = targetValue + " / " + maxValue;
        LeanTween.scale(slider.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
        LeanTween.color(slider.fillRect.gameObject, Color.white, 1f).setEase(LeanTweenType.easeInOutQuad);


        //Time.timeScale = 0;

    }

    private void CheckGameWinCondition()
    {
        if (enemyCount <= 0 && houseCount <= 0 && !isGameWin)
        {
            isGameWin = true;
            GameManager.Instance.GameOverActions();
        }
    }
    [System.Serializable]
    public class ScoreEntry
    {
        public string Name;
        public int Score;
    }
    private void UpdateLeaderboard(string playerName, int elapsedTime)
    {
        // Liderlik tablosunu yükle
        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
        for (int i = 0; i < MaxEntries; i++)
        {
            string name = PlayerPrefs.GetString("user" + i, "Unknown");
            int score = PlayerPrefs.GetInt("score" + i, 3600);
            scoreEntries.Add(new ScoreEntry { Name = name, Score = score });
        }

        // Yeni skoru oluþtur
        ScoreEntry newEntry = new ScoreEntry { Name = playerName, Score = elapsedTime };

        // Liderlik tablosuna yeni skoru ekle ve sýrala
        scoreEntries.Add(newEntry);
        scoreEntries.Sort((a, b) => a.Score.CompareTo(b.Score));
        if (scoreEntries.Count > MaxEntries)
        {
            scoreEntries.RemoveAt(MaxEntries);
        }

        // Liderlik tablosunu kaydet
        for (int i = 0; i < scoreEntries.Count; i++)
        {
            PlayerPrefs.SetString("user" + i, scoreEntries[i].Name);
            PlayerPrefs.SetInt("score" + i, scoreEntries[i].Score);
        }
    }
}
