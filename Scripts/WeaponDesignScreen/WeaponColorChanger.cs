// WeaponColorChanger.cs
using UnityEngine;

public class WeaponColorChanger : MonoBehaviour
{
    public Renderer weaponRenderer; // Silahýn renderer'ý
    private int colorIndex = 0; // Material index'i
    public MeshRenderer meshRenderer;
   // public static WeaponColorChanger Instance { get; private set; }

    public WeaponColorChanger weaponColorChanger;

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found!");
            return;
        }

        LoadSavedColors();
        ApplySavedColor();
    }

    public void ChangeMaterialColors(Color newColor)
    {

        SetMaterialColor(colorIndex, newColor);
        SaveColor(colorIndex, newColor);
    }

    public void ChangeMaterialIndex(int newIndex)
    {
        colorIndex = newIndex;
        ApplySavedColor();
    }

    void LoadSavedColors()
    {
        for (int i = 0; i < weaponRenderer.materials.Length; i++)
        {
            float r = PlayerPrefs.GetFloat($"WeaponColor_{i}_R", 1f);
            float g = PlayerPrefs.GetFloat($"WeaponColor_{i}_G", 1f);
            float b = PlayerPrefs.GetFloat($"WeaponColor_{i}_B", 1f);

            Color savedColor = new Color(r, g, b);
            SetMaterialColor(i, savedColor);
        }
    }

    void ApplySavedColor()
    {
        if (colorIndex >= 0 && colorIndex < weaponRenderer.materials.Length)
        {
            Color savedColor = GetMaterialColor(colorIndex);
            SetMaterialColor(colorIndex, savedColor);
        }
    }
    void SetMaterialColor(int index, Color color)
    {
        if (index >= 0 && index < weaponRenderer.materials.Length)
        {
            Material[] materials = weaponRenderer.materials;
            materials[index].color = color;

            // Emission renklerini de güncelle (isteðe baðlý)
            Color emissionColor = color * 0.5f;
            materials[index].SetColor("_EmissionColor", emissionColor);
            materials[index].EnableKeyword("_EMISSION");

            // Deðiþtirilen material'larý geri yükle
            weaponRenderer.materials = materials;
        }
    }

   public Color GetMaterialColor(int index)
    {
        if (index >= 0 && index < weaponRenderer.materials.Length)
        {
            return weaponRenderer.materials[index].color;
        }
        return Color.white;
    }

    void SaveColor(int index, Color color)
    {
        PlayerPrefs.SetFloat($"WeaponColor_{index}_R", color.r);
        PlayerPrefs.SetFloat($"WeaponColor_{index}_G", color.g);
        PlayerPrefs.SetFloat($"WeaponColor_{index}_B", color.b);
        PlayerPrefs.Save();
    }
}
