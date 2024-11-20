using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;

    private Vector2 touchStartPos;

    void Update()
    {
        HandleRotationInput();
    }

    void HandleRotationInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float rotateAmountX = (touch.position.x - touchStartPos.x) * rotationSpeed * Time.deltaTime;
                float rotateAmountY = (touch.position.y - touchStartPos.y) * rotationSpeed * Time.deltaTime;

                // Yatay (x) ekseni etrafýnda dönme
                transform.Rotate(Vector3.up, -rotateAmountX, Space.World);

                // Dikey (y) ekseni etrafýnda dönme
                transform.Rotate(Vector3.right, rotateAmountY, Space.World);

                touchStartPos = touch.position;
            }
        }
    }
}
