using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ClothingSet
{
    public string name;
    public Mesh mesh;
    public Material[] materials;
}

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer bodyRenderer;
    [SerializeField] SkinnedMeshRenderer legsRenderer;
    [SerializeField] SkinnedMeshRenderer feetRenderer;
    [SerializeField] SkinnedMeshRenderer headRenderer;

    public List<ClothingSet> bodyClothes;
    public List<ClothingSet> legsClothes;
    public List<ClothingSet> feetClothes;
    public List<ClothingSet> headClothes;

    private int randomIndex;
    private void Awake()
    {
        int savedIndex = PlayerPrefs.GetInt("SelectedClothingIndex", 0);
        // Örnek: Baþlangýçta bir kýyafet setini uygula
        ApplyClothing(bodyRenderer, bodyClothes[savedIndex]);
        ApplyClothing(legsRenderer, legsClothes[savedIndex]);
        ApplyClothing(feetRenderer, feetClothes[savedIndex]);
        ApplyClothing(headRenderer, headClothes[savedIndex]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Home"); // "MainMenu" yerine kendi ana menü sahnenizin adýný yazýn
    }
    public void ChangeClothing(SkinnedMeshRenderer renderer, List<ClothingSet> clothingList, int index)
    {
        if (index >= 0 && index < clothingList.Count)
        {
            ApplyClothing(renderer, clothingList[index]);
        }
        else
        {
            Debug.LogError("Invalid index for clothing selection!");
        }
    }

    private void ApplyClothing(SkinnedMeshRenderer renderer, ClothingSet clothingSet)
    {
        //Debug.Log($"Applying clothing: {clothingSet.name}");
        renderer.sharedMesh = clothingSet.mesh;

       // Debug.Log($"Material count: {clothingSet.materials.Length}");
        renderer.materials = clothingSet.materials;
    }
    private void SetRandomIndex()
    {
        randomIndex = Random.Range(0, bodyClothes.Count);
        Debug.Log($"RandomIndex: {randomIndex}");
    }

}
