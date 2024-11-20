using UnityEngine;

public class ChangeWeaponColorForGameScene : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        ApplyColorsToOtherWeapon();
    }

    void ApplyColorsToOtherWeapon()
    {
        Material[] materials = meshRenderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            float r = PlayerPrefs.GetFloat($"WeaponColor_{i}_R", 1f);
            float g = PlayerPrefs.GetFloat($"WeaponColor_{i}_G", 1f);
            float b = PlayerPrefs.GetFloat($"WeaponColor_{i}_B", 1f);

            Color savedColor = new Color(r, g, b);
            SetMaterialColor(i, savedColor);
        }

        meshRenderer.materials = materials;
    }

    void SetMaterialColor(int index, Color color)
    {
        if (index >= 0 && index < meshRenderer.materials.Length)
        {
            Material[] materials = meshRenderer.materials;
            materials[index].color = color;

            // Emission renklerini de güncelle (isteðe baðlý)
            Color emissionColor = color * 0.5f;
            materials[index].SetColor("_EmissionColor", emissionColor);
            materials[index].EnableKeyword("_EMISSION");

            // Deðiþtirilen material'larý geri yükle
            meshRenderer.materials = materials;
        }
    }
}
