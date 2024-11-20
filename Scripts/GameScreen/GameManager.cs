using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Canvas thirdPersonCanvas;
    public Canvas gameOverCanvas;
    public Canvas aimCanvas;
    public Canvas gameSceneCanvas;
    bool isGameOver = false;
    private AudioManager audioManager;
    private GameTweenAnimation gameTweenAnimation;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    private void Awake()
    {
        gameOverCanvas.enabled = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        gameTweenAnimation = FindObjectOfType<GameTweenAnimation>();

    }

    void Update()
    {
        // Oyun durumu kontrolü
        if (isGameOver)
        {
            SetGameOver();
            GameOverActions();
        }
    }

    public void SetGameOver()
    {
        isGameOver = false; // Oyunun bittiðini belirtmek için true yapýlmalý
    }

    public void GameOverActions()
    {
 
        CloseOtherCanvases();
        if(ScoreManager.instance.isGameWin)
        {
            gameTweenAnimation.RotateWinBackground();
            gameTweenAnimation.ShowWinHeaderPanel();
            audioManager.GameWinAudioSource();
        }
        else
        {
            OpenGameOverCanvas();
            audioManager.GameOverAudioSource();
        }
        
        
        
        Bullet.bulletCount = 0;
        Bullet.spareBulletCount = 0;
        PlayerController.playerHealth = 100;
        
    }

    void OpenGameOverCanvas()
    {
        gameOverCanvas.enabled = true;
        gameTweenAnimation.RotateGameOverBackground();
        gameTweenAnimation.ShowGameOverHeaderPanel();

    }

    void CloseOtherCanvases()
    {
        thirdPersonCanvas.enabled = false;
        aimCanvas.enabled = false;
        gameSceneCanvas.enabled = false;
        
    }

}
