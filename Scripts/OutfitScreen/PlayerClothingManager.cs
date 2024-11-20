using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClothingManager : MonoBehaviour
{
    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject shoes;

    [SerializeField] private List<Mesh> hairStyles;
    [SerializeField] private List<Mesh> topStyles;
    [SerializeField] private List<Mesh> shoesStyles;

    public Button rightButton;
    public Button leftButton;

    private int currentHairStyleIndex = 0;
    private int currentTopStyleIndex = 0;
    private int currentShoesStyleIndex = 0;

    // PlayerPref anahtarlarý
    private string hairStyleKey = "HairStyle";
    private string topStyleKey = "TopStyle";
    private string shoesStyleKey = "ShoesStyle";

    void Start()
    {
        // Kaydedilmiþ kýyafet stillerini kontrol et
        if (PlayerPrefs.HasKey(hairStyleKey))
        {
            currentHairStyleIndex = PlayerPrefs.GetInt(hairStyleKey);
        }

        if (PlayerPrefs.HasKey(topStyleKey))
        {
            currentTopStyleIndex = PlayerPrefs.GetInt(topStyleKey);
        }

        if (PlayerPrefs.HasKey(shoesStyleKey))
        {
            currentShoesStyleIndex = PlayerPrefs.GetInt(shoesStyleKey);
        }

        SetHairStyle(currentHairStyleIndex);
        SetTopStyle(currentTopStyleIndex);
        SetShoesStyle(currentShoesStyleIndex);

        // Butonlara týklama olaylarýný atama
        rightButton.onClick.AddListener(ChangeClothesRight);
        leftButton.onClick.AddListener(ChangeClothesLeft);
    }

    void ChangeClothesRight()
    {
        currentHairStyleIndex = (currentHairStyleIndex + 1) % hairStyles.Count;
        currentTopStyleIndex = (currentTopStyleIndex + 1) % topStyles.Count;
        currentShoesStyleIndex = (currentShoesStyleIndex + 1) % shoesStyles.Count;

        SavePlayerPrefs(); // Kýyafet tercihlerini kaydet

        SetHairStyle(currentHairStyleIndex);
        SetTopStyle(currentTopStyleIndex);
        SetShoesStyle(currentShoesStyleIndex);
    }

    void ChangeClothesLeft()
    {
        currentHairStyleIndex = (currentHairStyleIndex - 1 + hairStyles.Count) % hairStyles.Count;
        currentTopStyleIndex = (currentTopStyleIndex - 1 + topStyles.Count) % topStyles.Count;
        currentShoesStyleIndex = (currentShoesStyleIndex - 1 + shoesStyles.Count) % shoesStyles.Count;

        SavePlayerPrefs(); // Kýyafet tercihlerini kaydet

        SetHairStyle(currentHairStyleIndex);
        SetTopStyle(currentTopStyleIndex);
        SetShoesStyle(currentShoesStyleIndex);
    }

    void SavePlayerPrefs()
    {
        // Kýyafet tercihlerini kaydet
        PlayerPrefs.SetInt(hairStyleKey, currentHairStyleIndex);
        PlayerPrefs.SetInt(topStyleKey, currentTopStyleIndex);
        PlayerPrefs.SetInt(shoesStyleKey, currentShoesStyleIndex);
        PlayerPrefs.Save(); // PlayerPrefs verilerini kaydet
    }

    public void SetHairStyle(int index)
    {
        if (hairStyles.Count > 0 && index < hairStyles.Count)
        {
            Mesh hairMesh = hairStyles[index];
            SetMeshRendererMesh(hair, hairMesh);
        }
    }

    public void SetTopStyle(int index)
    {
        if (topStyles.Count > 0 && index < topStyles.Count)
        {
            Mesh topMesh = topStyles[index];
            SetMeshRendererMesh(top, topMesh);
        }
    }

    public void SetShoesStyle(int index)
    {
        if (shoesStyles.Count > 0 && index < shoesStyles.Count)
        {
            Mesh shoesMesh = shoesStyles[index];
            SetMeshRendererMesh(shoes, shoesMesh);
        }
    }

    void SetMeshRendererMesh(GameObject clothingObject, Mesh mesh)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = clothingObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.sharedMesh = mesh;
        }
    }


}
