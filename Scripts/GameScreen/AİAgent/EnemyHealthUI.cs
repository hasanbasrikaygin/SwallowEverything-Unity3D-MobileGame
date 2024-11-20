using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health enemyHealth; // D��man sa�l�k bile�eni referans�
    public Image filledImage; // Dolu durumu temsil eden image
    public Image emptyImage; // Bo� durumu temsil eden image

    private Transform player; // Oyuncu referans�
    private Transform enemyTransform; // D��man�n transformu



    private void Start()
    {
        // Ba�lang��ta gerekli bile�enleri kontrol et
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component is not assigned!");
            return;
        }
        //enemyHealth.maxHealth = 100;
        if (filledImage == null || emptyImage == null)
        {
            Debug.LogError("FilledImage or EmptyImage is not assigned!");
            return;
        }

        // Oyuncu referans�n� al
        player = Camera.main.transform; // Varsay�lan kameray� kullan�yoruz
        enemyTransform = enemyHealth.transform;
    }

    private void Update()
    {
        // Sa�l�k bile�eni ve g�r�nt�lerin varl���n� kontrol et
        if (enemyHealth != null && filledImage != null && emptyImage != null)
        {
            // Sa�l�k �ubu�unu g�ncelle
            UpdateHealthBar();

            // Sa�l�k �ubu�unu sadece oyuncuya do�ru d�nd�r
            RotateHealthBarTowardsPlayer();


        }

    }

    // Sa�l�k �ubu�unu g�ncelleyen fonksiyon
    void UpdateHealthBar()
    {
        float fillAmount = enemyHealth.CurrentHealth / enemyHealth.maxHealth;

        // Dolu durumu temsil eden Image'i g�ncelle
        filledImage.fillAmount = fillAmount;

        // Bo� durumu temsil eden Image'i g�ncelle
        emptyImage.fillAmount = 1 - fillAmount;
    }

    // Sa�l�k �ubu�unu oyuncuya do�ru d�nd�ren fonksiyon
    void RotateHealthBarTowardsPlayer()
    {
        // Sa�l�k �ubu�unu oyuncuya do�ru d�nd�r
        Vector3 toPlayerDirection = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);

        // D��man�n yaln�zca yatay d�n���n� kullanarak bak�� a��s�ndan ba��ms�z yap
        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }
    void PlaceHealthBarAboveEnemy()
    {
        Vector3 offset = new Vector3(0f, 2f, 0f); // Sa�l�k �ubu�unun d��man�n �st�nde olaca�� ofset

        // Sa�l�k �ubu�unun pozisyonunu d��man�n pozisyonuna g�re g�ncelle
        transform.position = enemyTransform.position + offset;
    }

}
