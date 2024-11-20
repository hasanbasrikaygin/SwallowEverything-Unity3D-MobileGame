using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health enemyHealth; // Düþman saðlýk bileþeni referansý
    public Image filledImage; // Dolu durumu temsil eden image
    public Image emptyImage; // Boþ durumu temsil eden image

    private Transform player; // Oyuncu referansý
    private Transform enemyTransform; // Düþmanýn transformu



    private void Start()
    {
        // Baþlangýçta gerekli bileþenleri kontrol et
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

        // Oyuncu referansýný al
        player = Camera.main.transform; // Varsayýlan kamerayý kullanýyoruz
        enemyTransform = enemyHealth.transform;
    }

    private void Update()
    {
        // Saðlýk bileþeni ve görüntülerin varlýðýný kontrol et
        if (enemyHealth != null && filledImage != null && emptyImage != null)
        {
            // Saðlýk çubuðunu güncelle
            UpdateHealthBar();

            // Saðlýk çubuðunu sadece oyuncuya doðru döndür
            RotateHealthBarTowardsPlayer();


        }

    }

    // Saðlýk çubuðunu güncelleyen fonksiyon
    void UpdateHealthBar()
    {
        float fillAmount = enemyHealth.CurrentHealth / enemyHealth.maxHealth;

        // Dolu durumu temsil eden Image'i güncelle
        filledImage.fillAmount = fillAmount;

        // Boþ durumu temsil eden Image'i güncelle
        emptyImage.fillAmount = 1 - fillAmount;
    }

    // Saðlýk çubuðunu oyuncuya doðru döndüren fonksiyon
    void RotateHealthBarTowardsPlayer()
    {
        // Saðlýk çubuðunu oyuncuya doðru döndür
        Vector3 toPlayerDirection = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);

        // Düþmanýn yalnýzca yatay dönüþünü kullanarak bakýþ açýsýndan baðýmsýz yap
        transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
    }
    void PlaceHealthBarAboveEnemy()
    {
        Vector3 offset = new Vector3(0f, 2f, 0f); // Saðlýk çubuðunun düþmanýn üstünde olacaðý ofset

        // Saðlýk çubuðunun pozisyonunu düþmanýn pozisyonuna göre güncelle
        transform.position = enemyTransform.position + offset;
    }

}
