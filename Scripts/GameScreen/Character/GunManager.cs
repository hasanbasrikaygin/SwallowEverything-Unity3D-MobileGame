using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Transform objTransform;
    public GameObject[] weapon;
    private string weaponID;
    public GameObject parentObj;
    MeshRenderer newWeaponRenderer;
    public  Material[] materials = new Material[34];
    public Material[] materialsss = new Material[5];
    public Material fiveMaterial;
    void Start()
    {
        weaponID = PlayerPrefs.GetString("GunID","blasterA");
        //Debug.Log(" PlayerPrefs.GetString(GunID) => " + weaponID);
        foreach (GameObject obj in weapon)
        {
            //Debug.Log("obj.name =>  " + obj.name);
            
            if (obj.name == weaponID)
            {
                
                GameObject newWeapon = Instantiate(obj, parentObj.transform);
                //obj.transform.SetParent(parentObj.transform);

                newWeapon.transform.position = objTransform.position;
                newWeapon.transform.rotation = objTransform.rotation;
                newWeapon.transform.localScale = objTransform.localScale;
                 newWeaponRenderer = newWeapon.GetComponent<MeshRenderer>();
                if(newWeaponRenderer == null)
                {
                    //Debug.Log("mesh renderer null");
                }
                else
                {
                   // Debug.Log("mesh renderer null de�il !!!!!");
                }
                
                string[] materialNames = new string[4];

                for (int i = 0; i < 4; i++)
                {
                    materialNames[i] = PlayerPrefs.GetString(SelectedWeapon.gunID +"-"+ i.ToString());
                   // Debug.Log("-"+materialNames[i]+"-");
                    Material loadedMaterial = Resources.Load<Material>("Materials/"+materialNames[i]);
                   // Debug.Log(loadedMaterial.name);
                    // Materyal var m� yok mu kontrol edin
                    if (loadedMaterial != null)
                    {
                        // Silah�n MeshRenderer bile�enine eri�in
                        MeshRenderer renderer = newWeapon.GetComponent<MeshRenderer>();

                        // Silah�n malzemelerinin listesini al�n
                        Material[] weaponMaterials = renderer.materials;

                        // Y�klenen materyali silah�n malzemesine atay�n
                        weaponMaterials[i] = loadedMaterial;

                        // Silah�n malzemelerini g�ncelleyin
                        renderer.materials = weaponMaterials;

                        // Materyali bulundu�unda yap�lacak i�lemler
                        Debug.Log("Materyal bulundu: " + loadedMaterial.name);
                    }
                    else
                    {
                        // Materyal bulunamad���nda yap�lacak i�lemler
                        //Debug.LogError("Materyal bulunamad�: " + loadedMaterial.name);
                    }
                }
                //if (newWeaponRenderer.materials[5] != null)
                //{
                //    newWeaponRenderer.materials[5].color = fiveMaterial.color;
                //}
                



            }
        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    string[] materialNames = new string[4];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        materialNames[i] = PlayerPrefs.GetString(i.ToString());
        //    }

        //    // Materyalleri e�le�tir ve atama yap
        //    for (int i = 0; i < 4; i++)
        //    {
        //        bool foundMaterial = false;
        //        foreach (Material material in materials)
        //        {
        //            if (material.name == materialNames[i])
        //            {
        //                Debug.Log("E�le�en materyal bulundu: " + material.name);
        //                newWeaponRenderer.materials[i] = material;
        //                foundMaterial = true;
        //                break;
        //            }
        //        }
        //        if (!foundMaterial)
        //        {
        //            Debug.LogError("E�le�en materyal bulunamad�: " + materialNames[i]);
        //        }
        //    }

        //    // Atanan materyal isimlerini kontrol et
        //    Debug.Log("Atanan Materyaller:");
        //    foreach (Material material in newWeaponRenderer.materials)
        //    {
        //        Debug.Log(material.name);
        //    }
        //}
        
    }
}
