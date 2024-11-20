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
        // LocalizationManager'� bul
        LocalizationManager manager = FindObjectOfType<LocalizationManager>();

        if (manager == null)
        {
            Debug.LogError("LocalizationManager not found in the scene.");
            yield break;
        }

        // Dil dosyas�n� y�kle
        manager.LoadSelectedLanguage();

        // Dil dosyas�n�n y�klenmesini bekleyin
        while (!manager.GetIsReady())
        {
            yield return null;
        }

        // Ana men� sahnesine ge�i� yap�n
        SceneManager.LoadScene("Home");
    }
}
