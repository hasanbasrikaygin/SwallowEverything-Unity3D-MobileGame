using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float shrinkSpeed = 0.01f; // Küçülme hýzý
    public float minDistance = 1f; // Minimum mesafe
    public float shrinkPercentage = 0.4f; // %30 küçülme
    private float initialScale; // Baþlangýç ölçeði
    private bool isShrinking = false;
    private bool hasBullet = false;
    [SerializeField] private int remainingShots = 3; // Kalan mermi sayýsý
    private Vector3 originalScale; // Baþlangýç ölçeðini vektör olarak tut
    private Transform pivot; // Odak noktasý
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public float growthFactor = 1f; // Büyüme faktörü
    public float growthDuration = 0.02f; // Büyüme süresi
    private PlayerBulletPool bulletPool;
    
    public AudioManager audioManager;
    private void Awake()
    {
        
        if(audioManager == null)
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        originalScale = transform.localScale;
        pivot = new GameObject("Pivot").transform;
        pivot.parent = transform;
        pivot.localPosition = Vector3.zero;
        bulletPool = FindObjectOfType<PlayerBulletPool>();
    }

    void Start()
    {
        initialScale = originalScale.x;

    }

    void Update()
    {
        if (isShrinking)
        {
            StartCoroutine(ShrinkBuilding());

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerBulletTag) || other.CompareTag(EnemyBulletTag) && !hasBullet)
        {
            pivot.position = other.transform.position;
            initialScale = transform.localScale.x;
            isShrinking = true;
            remainingShots--;

            if (TryGetComponent(out ResizeableObject component))
                component.TakeDamageAnim();
            audioManager.BuildingDamageAudioSource();
            //audioSource.PlayOneShot(audioManager.buildingDamageClip);
            if (remainingShots <= 0)
            {
                shrinkPercentage = 0.7f;
                //Debug.Log("Remaining shots zero, decreasing house count...");
                audioManager.BuildingDestroyAudioSource();
                //audioSource.PlayOneShot(audioManager.buildingDestroyClip);
                StartCoroutine(DestroyBuilding(gameObject.tag));
                hasBullet = true;
            }

            if (bulletPool != null)
            {
                bulletPool.ReturnObject(other.gameObject); // Mermi nesnesini havuza geri döndür
            }
            else
            {
                Debug.Log("havuz yok"); // Eðer havuz yoksa mermiyi devre dýþý býrak
            }

        }
    }
    IEnumerator GrowAnimation()
    {
        yield return new WaitForSeconds(3f); // Ýlk bekleme süresi
        Vector3 targetScale = originalScale; // Hedef ölçek orijinal ölçek
        transform.localScale = Vector3.zero; // Baþlangýçta ölçek sýfýr
        Vector3 initialScale = Vector3.zero; // Baþlangýç ölçeði
        float elapsedTime = 0f;
        float growthDuration = 5f; // Büyüme süresi

        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Son olarak tam hedef ölçeðe ayarla

        if (TryGetComponent(out ResizeableObject component))
        {
            component.TakeDamageAnim();
        }
    }

    IEnumerator DestroyBuilding(string name)
    {
        
        
        // Bina yok olacak iþlemleri burada gerçekleþtirilebilir
        yield return new WaitForSeconds(1.2f); // Ýstediðiniz bir bekleme süresi ekleyebilirsiniz
        if(name == "House")
        {
            ScoreManager.instance.DecreaseHouseCount();
        }
        Destroy(gameObject);
    }
    IEnumerator ShrinkBuilding()
    {

        yield return new WaitForSeconds(.5f);

        float deltaTime = Time.deltaTime * 1f;

        float newScale = Mathf.Clamp(transform.localScale.x * (1 - deltaTime), initialScale * (1 - shrinkPercentage), initialScale);

        if (newScale < initialScale * (1 - shrinkPercentage))
        {
            isShrinking = false;
        }

        Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);
        transform.localScale = newScaleVector;
        hasBullet = true;
        // StartCoroutine(ContinuousShake());
    }
    IEnumerator ContinuousShake()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        while (true)
        {

            yield return StartCoroutine(ShakeBuilding());
            float waitTime = 2f; // Sallanma arasýndaki bekleme süresi
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator ShakeBuilding()
    {
        yield return new WaitForSeconds(0.3f);
        float shakeDuration = 0.02f; // Sallanma süresi
        float shakeMagnitude = 0.01f; // Sallanma miktarý

        Vector3 originalPosition = transform.position;

        float elapsedShakeTime = 0f;

        while (elapsedShakeTime < shakeDuration)
        {
            // Rastgele bir sallanma vektörü oluþtur
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Sallanmýþ pozisyonu ayarla
            transform.position = originalPosition + shakeOffset;

            elapsedShakeTime += Time.deltaTime;
            yield return null;
        }
        
        // Sallanma süresi bittikten sonra orijinal pozisyona geri dön
        transform.position = originalPosition;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}