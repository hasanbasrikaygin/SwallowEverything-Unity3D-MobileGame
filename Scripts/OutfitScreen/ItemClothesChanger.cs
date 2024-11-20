using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClothesChanger : MonoBehaviour
{
    [SerializeField] private Mesh itemStyle; 
    [SerializeField] private GameObject clothes;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            SetShoesStyle();
            gameObject.SetActive(false);
        }
    }

    void SetShoesStyle()
    {
        // Ayakkab� stilini belirli bir stile ayarla
        SetMeshRendererMesh(clothes, itemStyle);
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
}
