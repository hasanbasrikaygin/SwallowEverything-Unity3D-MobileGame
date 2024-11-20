using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterRotation : MonoBehaviour
{
    // Dokunma hareketlerini kontrol etmek i�in kullan�lacak hassasiyet
    public float rotationSpeed = 0.5f;

    // Dokunma giri�inin ba�lang�� pozisyonunu saklamak i�in de�i�ken
    private Vector2 touchStartPos;

    void Update()
    {
        // Dokunma giri�i varsa
        if (Input.touchCount > 0)
        {
            // �lk dokunma giri�ini al
            Touch touch = Input.GetTouch(0);

            // Dokunma hareketinin t�r�ne g�re i�lem yap
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Dokunma hareketinin ba�lang�� pozisyonunu kaydet
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    // Dokunma hareketinin son pozisyonunu al
                    Vector2 touchEndPos = touch.position;

                    // Dokunma hareketinin ba�lang�� ve son pozisyonlar� aras�ndaki fark� hesapla
                    float deltaX = touchEndPos.x - touchStartPos.x;

                    // Karakteri d�nd�r
                    transform.Rotate(Vector3.up * deltaX * rotationSpeed, Space.World);

                    // Dokunma hareketinin son pozisyonunu ba�lang�� pozisyonu olarak g�ncelle
                    touchStartPos = touchEndPos;
                    break;
            }
        }
    }
}
