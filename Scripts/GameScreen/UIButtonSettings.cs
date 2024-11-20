using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public Button saveButton;
    public ButtonManager buttonManager;

    private void Start()
    {
        saveButton.onClick.AddListener(() =>
        {
            buttonManager.SaveButtonPositions();
            Debug.Log("Buton konumlarý kaydedildi!");
            // Paneli kapat veya ana sahneye dön
        });
    }
}
