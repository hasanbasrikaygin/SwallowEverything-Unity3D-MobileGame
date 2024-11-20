using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private GameObject bulletDecal;
     public static float bulletSpeed;
    //[SerializeField] private int bulletDamageForEnemy = 60;
    private float timeToDestroy = 5f;

    // Kurþunun hedef noktasý
    public Vector3 target { get; set; }
    // Kurþunun bir þeye çarpýp çarpmadýðý bilgisi
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

        // Kurþunu hedefe doðru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
        // Eðer kurþun henüz bir þeye çarpmadýysa ve hedefe çok yaklaþtýysa, nesneyi yok et
        if ( !hit && Vector3.Distance(transform.position , target) < 0.01f){
            playerBulletPool.ReturnObject(gameObject);
            // Destroy(gameObject );
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Kurþunun bir þeye çarpmasý durumunda yapýlacak iþlemler
        // Örneðin, bir decal oluþturma
        //Instantiate(bulletDecal, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        
        playerBulletPool.ReturnObject(gameObject);
    }
}
