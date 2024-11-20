using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class CharacterClothingManager : MonoBehaviour
{
    public GameObject[] clothingArray;
    public TextMeshProUGUI clothingName;
    public string[] clothingNameArray;
    private int currentClothingIndex;
    private string homeScene= "Home";
    public Button rightButton;
    public Button leftButton;
    public Button backButton;

    void Start()
    {
        currentClothingIndex = 0; // Ba�lang��ta varsay�lan k�yafeti etkinle�tir
        UpdateClothing();

        // Sa� ve sol butonlara fonksiyonlar� ekleyin
        rightButton.onClick.AddListener(NextClothing);
        leftButton.onClick.AddListener(PreviousClothing);
        backButton.onClick.AddListener(ReturnHome);
    }
     void NextClothing()
    {
        currentClothingIndex = (currentClothingIndex + 1) % clothingArray.Length;
        UpdateClothing();
    }

     void PreviousClothing()
    {
        currentClothingIndex = (currentClothingIndex - 1 + clothingArray.Length) % clothingArray.Length;
        UpdateClothing();
    }
    private void UpdateClothing()
    {
        // T�m k�yafetleri devre d��� b�rak
        DisableAllClothing();
        // �stenen k�yafeti etkinle�tir
        clothingArray[currentClothingIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedClothingIndex", currentClothingIndex);
        if (currentClothingIndex >= 0 && currentClothingIndex < clothingNameArray.Length)
        {
            string clothingNameText = clothingNameArray[currentClothingIndex];
            clothingName.text = clothingNameText;
        }
        else
        {
            Debug.LogError("Ge�ersiz k�yafet indeksi: " + currentClothingIndex);
        }
    }

    private void DisableAllClothing()
    {
        foreach (GameObject clothing in clothingArray)
        {
            clothing.SetActive(false);
        }
    }
    private void ReturnHome()
    {
        SceneManager.LoadScene(homeScene);
    }
}
