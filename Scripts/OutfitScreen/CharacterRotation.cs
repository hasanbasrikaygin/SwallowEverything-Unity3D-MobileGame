using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterRotation : MonoBehaviour
{
    // Dokunma hareketlerini kontrol etmek için kullanýlacak hassasiyet
    public float rotationSpeed = 0.5f;

    // Dokunma giriþinin baþlangýç pozisyonunu saklamak için deðiþken
    private Vector2 touchStartPos;

    void Update()
    {
        // Dokunma giriþi varsa
        if (Input.touchCount > 0)
        {
            // Ýlk dokunma giriþini al
            Touch touch = Input.GetTouch(0);

            // Dokunma hareketinin türüne göre iþlem yap
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Dokunma hareketinin baþlangýç pozisyonunu kaydet
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    // Dokunma hareketinin son pozisyonunu al
                    Vector2 touchEndPos = touch.position;

                    // Dokunma hareketinin baþlangýç ve son pozisyonlarý arasýndaki farký hesapla
                    float deltaX = touchEndPos.x - touchStartPos.x;

                    // Karakteri döndür
                    transform.Rotate(Vector3.up * deltaX * rotationSpeed, Space.World);

                    // Dokunma hareketinin son pozisyonunu baþlangýç pozisyonu olarak güncelle
                    touchStartPos = touchEndPos;
                    break;
            }
        }
    }
}
