using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomClothes : MonoBehaviour
{

    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject shoes;

    [SerializeField] private List<Mesh> hairStyles;
    [SerializeField] private List<Mesh> topStyles;
    [SerializeField] private List<Mesh> shoesStyles;

    private void Start()
    {
        // Baþlangýçta rastgele kýyafetleri ayarla
        SetRandomHairStyle();
        SetRandomTopStyle();
        SetRandomShoesStyle();
    }

    private void Update()
    {
        // Klavye giriþlerini kontrol et
        HandleInput();
    }

    private void HandleInput()
    {
        // Klavyeden herhangi bir tuþa basýldýðýnda rastgele kýyafetleri ayarla
        if (Input.anyKeyDown)
        {
            SetRandomHairStyle();
            SetRandomTopStyle();
            SetRandomShoesStyle();
        }
    }

    private void SetRandomHairStyle()
    {
        int randomIndex = Random.Range(0, hairStyles.Count);
        SetHairStyle(randomIndex);
    }

    private void SetRandomTopStyle()
    {
        int randomIndex = Random.Range(0, topStyles.Count);
        SetTopStyle(randomIndex);
    }

    private void SetRandomShoesStyle()
    {
        int randomIndex = Random.Range(0, shoesStyles.Count);
        SetShoesStyle(randomIndex);
    }

    private void SetHairStyle(int index)
    {
        // Hair stilini deðiþtir
        if (hairStyles.Count > 0 && index < hairStyles.Count)
        {
            Mesh hairMesh = hairStyles[index];
            SetMeshRendererMesh(hair, hairMesh);
        }
    }

    private void SetTopStyle(int index)
    {
        // Top stilini deðiþtir
        if (topStyles.Count > 0 && index < topStyles.Count)
        {
            Mesh topMesh = topStyles[index];
            SetMeshRendererMesh(top, topMesh);
        }
    }

    private void SetShoesStyle(int index)
    {
        // Shoes stilini deðiþtir
        if (shoesStyles.Count > 0 && index < shoesStyles.Count)
        {
            Mesh shoesMesh = shoesStyles[index];
            SetMeshRendererMesh(shoes, shoesMesh);
        }
    }

    private void SetMeshRendererMesh(GameObject clothingObject, Mesh mesh)
    {
        // SkinnedMeshRenderer'ýn mesh deðerini deðiþtir
        SkinnedMeshRenderer skinnedMeshRenderer = clothingObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.sharedMesh = mesh;
        }
    }
}


