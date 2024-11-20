using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private int bulletDamageForPlayer = 20;
    [SerializeField] private GameObject muzzleFlashPrefab; // Muzzle flash efekti
    [SerializeField] private GameObject hitEffectPrefab; // Vuru� efekti
    [SerializeField] private AudioClip shootSound; // Ate�leme sesi
    [SerializeField] private AudioClip hitSound; // Vuru� sesi
    private AudioSource audioSource;
    private EnemyBulletPool pool;
    public GameObject bulletStartPoint;

    private void Start()
    {
        pool = transform.parent.GetComponent<EnemyBulletPool>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(Vector3 origin, Vector3 target, AiAgent agent)
    {
        Vector3 direction = (target - origin).normalized;
        RaycastHit hit;

        // Raycast kullanarak hedefi tespit et
        if (Physics.Raycast(origin, direction, out hit))
        {
            // Vuru� efekti olu�tur
           // Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            // Vuru� sesi �al
            //audioSource.PlayOneShot(hitSound);

            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerController>().TakeDamage(bulletDamageForPlayer);
            }
        }

        // Ate�leme efekti olu�tur
        //Instantiate(muzzleFlashPrefab, origin, Quaternion.identity);

        // Ate�leme sesi �al
        //audioSource.PlayOneShot(shootSound);

        // Mermiyi havuza geri d�nd�r
        //pool.ReturnObject(gameObject);
    }
}


//public class EnemyBulletController : MonoBehaviour
//{
//    [SerializeField] private float bulletSpeed = 1f;
//    [SerializeField] private int bulletDamageForPlayer = 20;
//    private Transform target;
//    private EnemyBulletPool pool;

//    private void Start()
//    {
//        target = GameObject.FindGameObjectWithTag("Player").transform;
//        pool = transform.parent.GetComponent<EnemyBulletPool>();
//    }

//    private void OnEnable()
//    {
//        StartCoroutine(DestroyBulletAfterTime());
//        ShootBulletTowardsPlayer();
//    }

//    private IEnumerator DestroyBulletAfterTime()
//    {
//        yield return new WaitForSeconds(2f);
//        pool.ReturnObject(gameObject);
//    }

//    private void ShootBulletTowardsPlayer()
//    {
//        Vector3 direction = (target.position - transform.position).normalized;
//        RaycastHit hit;

//        if (Physics.Raycast(transform.position, direction, out hit))
//        {
//            if (hit.collider.CompareTag("Player"))
//            {
//                target.GetComponent<PlayerController>().TakeDamage(bulletDamageForPlayer);
//                Debug.Log("Vuruldun");
//            }
//        }

//        // Hedefe do�ru hareket etmek yerine, mermiyi hedefe do�ru hareket ettirmeden �nce
//        // �arpt��� noktaya ta��yabiliriz.
//        transform.position = hit.point;
//        pool.ReturnObject(gameObject);
//    }
//}


////
//public class EnemyBulletController : MonoBehaviour
//{

//   // [SerializeField] private GameObject bulletDecal;
//    [SerializeField] private float bulletSpeed = 1f;
//    [SerializeField] private int bulletDamageForPlayer = 20;
//    private float timeToDestroy = 2f;
//    private float lifeTime = 2f; // Mermi �mr�
//    private float elapsedTime = 0f; // Ge�en s�re
//    //[SerializeField] ParticleSystem DeBuffSmokeEffect;
//    // Kur�unun hedef noktas�

//    public Transform target;
//    // Kur�unun bir �eye �arp�p �arpmad��� bilgisi
//    public bool hit { get; set; }
//    private Rigidbody rb;
//    EnemyBulletPool pool;
//    private Vector3 direction; // Hedefe do�ru hareket y�n�
//    private void Start()
//    {
//        if (target == null)
//        {
//            target = GameObject.FindGameObjectWithTag("Player").transform;

//        }
//        //rb = GetComponent<Rigidbody>();
//        //rb.AddForce((target - transform.position).normalized * bulletSpeed, ForceMode.Impulse);
//        pool = transform.parent.GetComponent<EnemyBulletPool>();


//    }
//    private void OnEnable()
//{

//    StartCoroutine(DestroyBulletAfterTime());
//}
//    IEnumerator DestroyBulletAfterTime()
//    {
//        yield return new WaitForSeconds(timeToDestroy);
//        pool.ReturnObject(gameObject);
//    }

//    void FixedUpdate()
//    {
//        direction = (target.position - transform.position).normalized;
//        transform.position += direction * bulletSpeed * Time.deltaTime;
//        elapsedTime += Time.deltaTime;
//        if (elapsedTime >= lifeTime)
//        {
//            pool.ReturnObject(gameObject);
//        }
//        //if (Vector3.Distance(transform.position, target.position) < 0.01f)

//        //{
//        //    pool.ReturnObject(gameObject);
//        //}
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (gameObject == null)
//        {
//            Debug.LogError("Trigger entered: gameObject is null!");
//            return;
//        }

//        if (other.tag == "Player")
//        {
//            other.GetComponent<PlayerController>().TakeDamage(bulletDamageForPlayer);
//            pool.ReturnObject(gameObject);
//        }

//    }

//    private Vector3 ApplyRandomDeviation(Vector3 originalTarget, AiAgent agent)
//    {
//        // Rastgele birim vekt�r al, bu vekt�r mermi y�n�n� etkileyecek
//        Vector3 randomDirection = Random.onUnitSphere;
//        randomDirection.y = 0f; // Y�n� yaln�zca yatay d�zlemde kullanmak i�in y eksenini s�f�rla
//        randomDirection.Normalize(); // Y�n vekt�r�n� birim uzunluklu yap

//        // Sapma miktar�n�, agent'�n belirtti�i aral�klarda kullanarak sapma vekt�r�n� hesapla
//        float randomSpreadX = Random.Range(-agent.randomSpreadX, agent.randomSpreadX);
//        float randomSpreadY = Random.Range(-agent.randomSpreadY, agent.randomSpreadY);
//        Vector3 randomDeviation = Quaternion.Euler(0, randomSpreadX, randomSpreadY) * randomDirection;

//        // Orjinal hedefe sapma vekt�r�n� ekle
//        return originalTarget + randomDeviation * 2f;
//    }

//}
