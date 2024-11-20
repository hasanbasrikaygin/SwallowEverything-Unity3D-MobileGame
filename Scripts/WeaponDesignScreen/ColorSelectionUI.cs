// ColorSelectionUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectionUI : MonoBehaviour
{
    public WeaponColorChanger weaponColorChanger;

    Color selectedColor;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            // Butona týklama olayýný ekle
            button.onClick.AddListener(ChangeColor);
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    void ChangeColor()
    {
        if (weaponColorChanger != null)
        {
            // Butonun rengini WeaponColorChanger'a ilet
            Color selectedColor = button.image.color;
            weaponColorChanger.ChangeMaterialColors(selectedColor);

        }
        else
        {
            Debug.LogError("WeaponColorChanger not assigned!");
        }
    }
}
