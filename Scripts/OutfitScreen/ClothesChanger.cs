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
        // Ba�lang��ta karakterin k�yafetlerini belirli bir stilde ba�latmak i�in
        //SetHairStyle(currentHairStyleIndex);
        //SetTopStyle(currentTopStyleIndex);
        //SetShoesStyle(currentShoesStyleIndex);
        SetClothesFromPlayerPrefs();
    }

    void Update()
    {
        // Klavye giri�lerini kontrol et
        HandleInput();
    } 

    void HandleInput()
    {
        // Hair de�i�tirme kontrol� (�rne�in "1" tu�u)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHairStyleIndex = (currentHairStyleIndex + 1) % hairStyles.Count;
            SetHairStyle(currentHairStyleIndex);
        }

        // Top de�i�tirme kontrol� (�rne�in "2" tu�u)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTopStyleIndex = (currentTopStyleIndex + 1) % topStyles.Count;
            SetTopStyle(currentTopStyleIndex);
        }

        // Shoes de�i�tirme kontrol� (�rne�in "3" tu�u)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentShoesStyleIndex = (currentShoesStyleIndex + 1) % shoesStyles.Count;
            SetShoesStyle(currentShoesStyleIndex);
        }
    }

    void SetHairStyle(int index)
    {
        // Hair stilini de�i�tir
        if (hairStyles.Count > 0 && index < hairStyles.Count)
        {
            Mesh hairMesh = hairStyles[index];
            SetMeshRendererMesh(hair, hairMesh);
        }
    }

    void SetTopStyle(int index)
    {
        // Top stilini de�i�tir
        if (topStyles.Count > 0 && index < topStyles.Count)
        {
            Mesh topMesh = topStyles[index];
            SetMeshRendererMesh(top, topMesh);
        }
    }

    void SetShoesStyle(int index)
    {
        // Shoes stilini de�i�tir
        if (shoesStyles.Count > 0 && index < shoesStyles.Count)
        {
            Mesh shoesMesh = shoesStyles[index];
            SetMeshRendererMesh(shoes, shoesMesh);
        }
    }

    void SetMeshRendererMesh(GameObject clothingObject, Mesh mesh)
    {
        // SkinnedMeshRenderer'�n mesh de�erini de�i�tir
        SkinnedMeshRenderer skinnedMeshRenderer = clothingObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.sharedMesh = mesh;
        }
    }
    void SetClothesFromPlayerPrefs()
    {
        // PlayerPrefs'ten kaydedilmi� k�yafet tercihlerini �ek
        int savedHairStyle = PlayerPrefs.GetInt("HairStyle", 0);
        int savedTopStyle = PlayerPrefs.GetInt("TopStyle", 0);
        int savedShoesStyle = PlayerPrefs.GetInt("ShoesStyle", 0);

        // Kaydedilmi� tercihleri uygula
        SetHairStyle(savedHairStyle);
        SetTopStyle(savedTopStyle);
        SetShoesStyle(savedShoesStyle);
    }
}

