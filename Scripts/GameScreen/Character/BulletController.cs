using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private GameObject bulletDecal;
     public static float bulletSpeed;
    //[SerializeField] private int bulletDamageForEnemy = 60;
    private float timeToDestroy = 5f;

    // Kur�unun hedef noktas�
    public Vector3 target { get; set; }
    // Kur�unun bir �eye �arp�p �arpmad��� bilgisi
    public bool hit {  get; set; }
   // EnemyController enemyController;
    PlayerBulletPool playerBulletPool;


    private void Start()
    {
        playerBulletPool = transform.parent.GetComponent<PlayerBulletPool>();
        
    }
    private void OnEnable()
    {
        StartCoroutine(DestroyBulletAfterTime());
    }
    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        playerBulletPool.ReturnObject(gameObject);
    }
    
    void FixedUpdate()
    {

        // Kur�unu hedefe do�ru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
        // E�er kur�un hen�z bir �eye �arpmad�ysa ve hedefe �ok yakla�t�ysa, nesneyi yok et
        if ( !hit && Vector3.Distance(transform.position , target) < 0.01f){
            playerBulletPool.ReturnObject(gameObject);
            // Destroy(gameObject );
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Kur�unun bir �eye �arpmas� durumunda yap�lacak i�lemler
        // �rne�in, bir decal olu�turma
        //Instantiate(bulletDecal, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        
        playerBulletPool.ReturnObject(gameObject);
    }
}
