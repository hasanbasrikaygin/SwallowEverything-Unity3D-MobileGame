using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private GameEndAdsManager gameEndAdsManager;
    private string menuScene = "Home";
    void Start()
    {
        homeButton.onClick.AddListener(OpenHome);
    }
    public void OpenHome()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (gameEndAdsManager.LoadAddIsNull())
            {
                gameEndAdsManager.LoadInterstitialAd();
            }
            gameEndAdsManager.ShowInterstitialAd();
        }
        else
        {
            SceneManager.LoadScene(menuScene);
        }
        
    }
}
