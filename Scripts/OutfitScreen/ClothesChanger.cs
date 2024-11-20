using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesChanger : MonoBehaviour
{
    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject shoes;

    [SerializeField] private List<Mesh> hairStyles;
    [SerializeField] private List<Mesh> topStyles;
    [SerializeField] private List<Mesh> shoesStyles;

    private int currentHairStyleIndex = 0;
    private int currentTopStyleIndex = 0;
    private int currentShoesStyleIndex = 0;

    void Start()
    {
        // Baþlangýçta karakterin kýyafetlerini belirli bir stilde baþlatmak için
        //SetHairStyle(currentHairStyleIndex);
        //SetTopStyle(currentTopStyleIndex);
        //SetShoesStyle(currentShoesStyleIndex);
        SetClothesFromPlayerPrefs();
    }

    void Update()
    {
        // Klavye giriþlerini kontrol et
        HandleInput();
    } 

    void HandleInput()
    {
        // Hair deðiþtirme kontrolü (örneðin "1" tuþu)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHairStyleIndex = (currentHairStyleIndex + 1) % hairStyles.Count;
            SetHairStyle(currentHairStyleIndex);
        }

        // Top deðiþtirme kontrolü (örneðin "2" tuþu)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTopStyleIndex = (currentTopStyleIndex + 1) % topStyles.Count;
            SetTopStyle(currentTopStyleIndex);
        }

        // Shoes deðiþtirme kontrolü (örneðin "3" tuþu)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentShoesStyleIndex = (currentShoesStyleIndex + 1) % shoesStyles.Count;
            SetShoesStyle(currentShoesStyleIndex);
        }
    }

    void SetHairStyle(int index)
    {
        // Hair stilini deðiþtir
        if (hairStyles.Count > 0 && index < hairStyles.Count)
        {
            Mesh hairMesh = hairStyles[index];
            SetMeshRendererMesh(hair, hairMesh);
        }
    }

    void SetTopStyle(int index)
    {
        // Top stilini deðiþtir
        if (topStyles.Count > 0 && index < topStyles.Count)
        {
            Mesh topMesh = topStyles[index];
            SetMeshRendererMesh(top, topMesh);
        }
    }

    void SetShoesStyle(int index)
    {
        // Shoes stilini deðiþtir
        if (shoesStyles.Count > 0 && index < shoesStyles.Count)
        {
            Mesh shoesMesh = shoesStyles[index];
            SetMeshRendererMesh(shoes, shoesMesh);
        }
    }

    void SetMeshRendererMesh(GameObject clothingObject, Mesh mesh)
    {
        // SkinnedMeshRenderer'ýn mesh deðerini deðiþtir
        SkinnedMeshRenderer skinnedMeshRenderer = clothingObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.sharedMesh = mesh;
        }
    }
    void SetClothesFromPlayerPrefs()
    {
        // PlayerPrefs'ten kaydedilmiþ kýyafet tercihlerini çek
        int savedHairStyle = PlayerPrefs.GetInt("HairStyle", 0);
        int savedTopStyle = PlayerPrefs.GetInt("TopStyle", 0);
        int savedShoesStyle = PlayerPrefs.GetInt("ShoesStyle", 0);

        // Kaydedilmiþ tercihleri uygula
        SetHairStyle(savedHairStyle);
        SetTopStyle(savedTopStyle);
        SetShoesStyle(savedShoesStyle);
    }
}

