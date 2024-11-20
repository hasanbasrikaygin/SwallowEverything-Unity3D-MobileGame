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
        currentClothingIndex = 0; // Baþlangýçta varsayýlan kýyafeti etkinleþtir
        UpdateClothing();

        // Sað ve sol butonlara fonksiyonlarý ekleyin
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
        // Tüm kýyafetleri devre dýþý býrak
        DisableAllClothing();
        // Ýstenen kýyafeti etkinleþtir
        clothingArray[currentClothingIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedClothingIndex", currentClothingIndex);
        if (currentClothingIndex >= 0 && currentClothingIndex < clothingNameArray.Length)
        {
            string clothingNameText = clothingNameArray[currentClothingIndex];
            clothingName.text = clothingNameText;
        }
        else
        {
            Debug.LogError("Geçersiz kýyafet indeksi: " + currentClothingIndex);
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
