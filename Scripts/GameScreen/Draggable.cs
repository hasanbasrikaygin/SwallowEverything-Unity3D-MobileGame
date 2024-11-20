using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private ButtonManager buttonManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        buttonManager = FindObjectOfType<ButtonManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        ClampToWindow();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonManager.SetSelectedButton(gameObject);
    }

    private void ClampToWindow()
    {
        Vector3[] panelCorners = new Vector3[4];
        buttonManager.adjustmentPanel.GetWorldCorners(panelCorners);

        Vector3[] buttonCorners = new Vector3[4];
        rectTransform.GetWorldCorners(buttonCorners);

        for (int i = 0; i < 4; i++)
        {
            buttonCorners[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buttonCorners[i]);
            panelCorners[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, panelCorners[i]);
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

        rectTransform.anchoredPosition += offset / canvas.scaleFactor;
    }
}