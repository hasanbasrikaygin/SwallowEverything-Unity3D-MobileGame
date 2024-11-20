using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    
    private void Start()
    {

        StartCoroutine(LoadGame());
      
    }

    private IEnumerator LoadGame()
    {
        // LocalizationManager'ý bul
        LocalizationManager manager = FindObjectOfType<LocalizationManager>();

        if (manager == null)
        {
            Debug.LogError("LocalizationManager not found in the scene.");
            yield break;
        }

        // Dil dosyasýný yükle
        manager.LoadSelectedLanguage();

        // Dil dosyasýnýn yüklenmesini bekleyin
        while (!manager.GetIsReady())
        {
            yield return null;
        }

        // Ana menü sahnesine geçiþ yapýn
        SceneManager.LoadScene("Home");
    }
}
