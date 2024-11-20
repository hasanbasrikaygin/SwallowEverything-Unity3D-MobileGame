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
        // Ayakkabý stilini belirli bir stile ayarla
        SetMeshRendererMesh(clothes, itemStyle);
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
}
