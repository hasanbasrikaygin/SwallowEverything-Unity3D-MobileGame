using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // Ayarlanan butonlar
    public GameObject settingJumpButton;
    public GameObject settingFireButton;
    public GameObject settingAimButton;
    public GameObject settingReloadButton;
    public GameObject settingAnotherFireButton;
    public GameObject settingMoveButton;

    // Ana sahnedeki butonlar
    public GameObject mainJumpButton;
    public GameObject mainFireButton;
    public GameObject mainAimButton;
    public GameObject mainReloadButton;
    public GameObject mainAnotherFireButton;
    public GameObject mainMoveButton;

    private List<GameObject> selectedBackgrounds;
    public GameObject jumpButtonSelectedBackground;
    public GameObject fireButtonSelectedBackground;
    public GameObject aimButtonSelectedBackground;
    public GameObject reloadButtonSelectedBackground;
    public GameObject anotherFireButtonSelectedBackground;
    public GameObject moveButtonSelectedBackground;

    // Slider ve seçilen buton
    public Slider sizeSlider;
    private GameObject selectedButton;

    // Ayarlama paneli
    public RectTransform adjustmentPanel;

    private void Start()
    {
        LoadButtonPositions();
        InitializeSizeSlider();
        selectedBackgrounds = new List<GameObject>
        {
            jumpButtonSelectedBackground,
            fireButtonSelectedBackground,
            aimButtonSelectedBackground,
            reloadButtonSelectedBackground,
            anotherFireButtonSelectedBackground,
            moveButtonSelectedBackground
        };
    }

    private void InitializeSizeSlider()
    {
        sizeSlider.minValue = 0.5f;
        sizeSlider.maxValue = 1.5f;
        sizeSlider.value = 1.0f; // Default deðer

        sizeSlider.onValueChanged.AddListener(UpdateButtonSize);
    }

    private void UpdateButtonSize(float newSize)
    {
        if (selectedButton != null)
        {
            selectedButton.transform.localScale = Vector3.one * newSize;
            ClampToSafeArea(selectedButton);
        }
    }

    public void SaveButtonPositions()
    {
        SaveButtonPositionAndScale("JumpButton", settingJumpButton);
        SaveButtonPositionAndScale("FireButton", settingFireButton);
        SaveButtonPositionAndScale("AimButton", settingAimButton);
        SaveButtonPositionAndScale("ReloadButton", settingReloadButton);
        SaveButtonPositionAndScale("AnotherFireButton", settingAnotherFireButton);
        SaveButtonPositionAndScale("MoveButton", settingMoveButton);
        PlayerPrefs.Save();
    }

    private void SaveButtonPositionAndScale(string buttonName, GameObject button)
    {
        PlayerPrefs.SetFloat(buttonName + "X", button.transform.position.x);
        PlayerPrefs.SetFloat(buttonName + "Y", button.transform.position.y);
        PlayerPrefs.SetFloat(buttonName + "Scale", button.transform.localScale.x);
    }

    public void LoadButtonPositions()
    {
        LoadButtonPositionAndScale("JumpButton", settingJumpButton, mainJumpButton);
        LoadButtonPositionAndScale("FireButton", settingFireButton, mainFireButton);
        LoadButtonPositionAndScale("AimButton", settingAimButton, mainAimButton);
        LoadButtonPositionAndScale("ReloadButton", settingReloadButton, mainReloadButton);
        LoadButtonPositionAndScale("AnotherFireButton", settingAnotherFireButton, mainAnotherFireButton);
        LoadButtonPositionAndScale("MoveButton", settingMoveButton, mainMoveButton);
    }

    private void LoadButtonPositionAndScale(string buttonName, GameObject settingButton, GameObject mainButton)
    {
        if (PlayerPrefs.HasKey(buttonName + "X"))
        {
            Vector2 newPosition = new Vector2(PlayerPrefs.GetFloat(buttonName + "X"), PlayerPrefs.GetFloat(buttonName + "Y"));
            float newScale = PlayerPrefs.GetFloat(buttonName + "Scale", 1.0f);

            settingButton.transform.position = newPosition;
            settingButton.transform.localScale = Vector3.one * newScale;

            mainButton.transform.position = newPosition;
            mainButton.transform.localScale = Vector3.one * newScale;
        }
    }
    public void SetSelectedButton(GameObject button)
    {
        selectedButton = button;
        sizeSlider.value = selectedButton.transform.localScale.x;

        // Seçili arka planý etkinleþtir ve diðerlerini devre dýþý býrak
        for (int i = 0; i < selectedBackgrounds.Count; i++)
        {
            selectedBackgrounds[i].SetActive(false);
            if (selectedBackgrounds[i].name == selectedButton.name + "BG")
                selectedBackgrounds[i].SetActive(true);
        }
    }

    private void ClampToSafeArea(GameObject button)
    {
        Vector3[] panelCorners = new Vector3[4];
        adjustmentPanel.GetWorldCorners(panelCorners);

        Vector3[] buttonCorners = new Vector3[4];
        button.GetComponent<RectTransform>().GetWorldCorners(buttonCorners);

        for (int i = 0; i < 4; i++)
        {
            buttonCorners[i] = RectTransformUtility.WorldToScreenPoint(null, buttonCorners[i]);
            panelCorners[i] = RectTransformUtility.WorldToScreenPoint(null, panelCorners[i]);
        }

        Vector2 offset = Vector2.zero;

        if (buttonCorners[0].x < panelCorners[0].x)
        {
            offset.x = panelCorners[0].x - buttonCorners[0].x;
        }
        else if (buttonCorners[2].x > panelCorners[2].x)
        {
            offset.x = panelCorners[2].x - buttonCorners[2].x;
        }

        if (buttonCorners[0].y < panelCorners[0].y)
        {
            offset.y = panelCorners[0].y - buttonCorners[0].y;
        }
        else if (buttonCorners[2].y > panelCorners[2].y)
        {
            offset.y = panelCorners[2].y - buttonCorners[2].y;
        }

        button.GetComponent<RectTransform>().anchoredPosition += offset / adjustmentPanel.localScale.x;
    }
}
