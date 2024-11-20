// MaterialIndexSelectionUI.cs
using UnityEngine;
using UnityEngine.UI;

public class MaterialIndexSelectionUI : MonoBehaviour
{
    public WeaponColorChanger weaponColorChanger;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            // Butona týklama olayýný ekle
            button.onClick.AddListener(ChangeIndex);
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    void ChangeIndex()
    {
        if (weaponColorChanger != null)
        {
            // Butonun indexini WeaponColorChanger'a ilet
            weaponColorChanger.ChangeMaterialIndex(transform.GetSiblingIndex());
        }
        else
        {
            Debug.LogError("WeaponColorChanger not assigned!");
        }
    }
}
