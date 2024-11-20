using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponColorUpdater : MonoBehaviour
{
    public MeshRenderer meshRenderer; // Silah�n MeshRenderer bile�eni
    public List<Button> styleButtons; // Her butonun listesi
    public List<WeaponColorOptions> weaponColorOptions; // Her silah i�in renk se�eneklerinin listesi

    void Start()
    {
        // Butonlara t�klanma olaylar�n� atay�n
        for (int i = 0; i < styleButtons.Count; i++)
        {
            int buttonIndex = i; // Bu butona t�kland���nda hangi renk se�eneklerinin uygulanaca��n� belirlemek i�in indis
            styleButtons[i].onClick.AddListener(() => ApplyColorOption(buttonIndex));
        }
    }

    // Belirli bir d��meye t�kland���nda �a�r�lacak fonksiyon
    public void ApplyColorOption(int buttonIndex)
    {
        if (gameObject.activeSelf)
        {
            // Butonun ba�l� oldu�u renk se�eneklerini al
            List<Material> selectedColors = weaponColorOptions[buttonIndex].colorOptions;

            // Yeni malzemeleri atama
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            // Materyallerin listesini al
            Material[] materials = renderer.materials;

            // Her bir materyali yeni materyalle de�i�tir
            for (int i = 0; i < materials.Length; i++)
            {
              //  materials[i] = newMaterial;
            }

            // Yeni materyalleri objeye uygula
            renderer.materials = materials;
            for (int i = 0; i < selectedColors.Count && i < meshRenderer.materials.Length; i++)
            {
                if (selectedColors[i] != null)
                {
                    materials[i] = selectedColors[i];
                    meshRenderer.materials[i] = selectedColors[i];
                    Debug.Log(selectedColors[i].name);
                    string selectedMaterialName = selectedColors[i].name;
                      
                       selectedMaterialName = selectedMaterialName.Split(' ')[0];
                       Debug.Log("renk : " + selectedMaterialName);
                       PlayerPrefs.SetString(SelectedWeapon.gunID+ "-" + i, selectedMaterialName);

                }
            }
            renderer.materials = materials;
            
        }
    }
}

[System.Serializable]
public class WeaponColorOptions
{
    public List<Material> colorOptions; // Belirli bir silah i�in renk se�eneklerinin listesi
}
//for (int i = 0; i < 4; i++)
//{
//    // Silah�n malzemeleri �zerinde de�i�iklik yaparak renkleri g�ncelle
//    if (weaponObjects[selectedWeaponIndex].GetComponent<MeshRenderer>() == null)
//    {

//    }
//    Debug.Log("1. " + weaponObjects[selectedWeaponIndex].GetComponent<MeshRenderer>().materials[i].name);
//    Debug.Log("2. " + weapons[selectedWeaponIndex].weapon.GetComponent<MeshRenderer>().materials[i].name);
//    string selectedMaterialName = weaponObjects[selectedWeaponIndex].GetComponent<MeshRenderer>().materials[i].name;
//    //string selectedMaterialName = weapons[selectedWeaponIndex].weapon.GetComponent<MeshRenderer>().materials[i].name;
//    selectedMaterialName = selectedMaterialName.Split(' ')[0];
//    Debug.Log("renk : " + selectedMaterialName);
//    PlayerPrefs.SetString("" + i, selectedMaterialName);

//}