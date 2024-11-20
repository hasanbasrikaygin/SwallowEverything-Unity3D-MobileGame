using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private int bulletDamageForPlayer = 20;
    [SerializeField] private GameObject muzzleFlashPrefab; // Muzzle flash efekti
    [SerializeField] private GameObject hitEffectPrefab; // Vuruþ efekti
    [SerializeField] private AudioClip shootSound; // Ateþleme sesi
    [SerializeField] private AudioClip hitSound; // Vuruþ sesi
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
            // Vuruþ efekti oluþtur
           // Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            // Vuruþ sesi çal
            //audioSource.PlayOneShot(hitSound);

            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerController>().TakeDamage(bulletDamageForPlayer);
            }
        }

        // Ateþleme efekti oluþtur
        //Instantiate(muzzleFlashPrefab, origin, Quaternion.identity);

        // Ateþleme sesi çal
        //audioSource.PlayOneShot(shootSound);

        // Mermiyi havuza geri döndür
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

//        // Hedefe doðru hareket etmek yerine, mermiyi hedefe doðru hareket ettirmeden önce
//        // çarptýðý noktaya taþýyabiliriz.
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
//    private float lifeTime = 2f; // Mermi ömrü
//    private float elapsedTime = 0f; // Geçen süre
//    //[SerializeField] ParticleSystem DeBuffSmokeEffect;
//    // Kurþunun hedef noktasý

//    public Transform target;
//    // Kurþunun bir þeye çarpýp çarpmadýðý bilgisi
//    public bool hit { get; set; }
//    private Rigidbody rb;
//    EnemyBulletPool pool;
//    private Vector3 direction; // Hedefe doðru hareket yönü
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
//        // Rastgele birim vektör al, bu vektör mermi yönünü etkileyecek
//        Vector3 randomDirection = Random.onUnitSphere;
//        randomDirection.y = 0f; // Yönü yalnýzca yatay düzlemde kullanmak için y eksenini sýfýrla
//        randomDirection.Normalize(); // Yön vektörünü birim uzunluklu yap

//        // Sapma miktarýný, agent'ýn belirttiði aralýklarda kullanarak sapma vektörünü hesapla
//        float randomSpreadX = Random.Range(-agent.randomSpreadX, agent.randomSpreadX);
//        float randomSpreadY = Random.Range(-agent.randomSpreadY, agent.randomSpreadY);
//        Vector3 randomDeviation = Quaternion.Euler(0, randomSpreadX, randomSpreadY) * randomDirection;

//        // Orjinal hedefe sapma vektörünü ekle
//        return originalTarget + randomDeviation * 2f;
//    }

//}
