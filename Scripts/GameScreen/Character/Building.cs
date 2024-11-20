using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float shrinkSpeed = 0.01f; // K���lme h�z�
    public float minDistance = 1f; // Minimum mesafe
    public float shrinkPercentage = 0.4f; // %30 k���lme
    private float initialScale; // Ba�lang�� �l�e�i
    private bool isShrinking = false;
    private bool hasBullet = false;
    [SerializeField] private int remainingShots = 3; // Kalan mermi say�s�
    private Vector3 originalScale; // Ba�lang�� �l�e�ini vekt�r olarak tut
    private Transform pivot; // Odak noktas�
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public float growthFactor = 1f; // B�y�me fakt�r�
    public float growthDuration = 0.02f; // B�y�me s�resi
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
                bulletPool.ReturnObject(other.gameObject); // Mermi nesnesini havuza geri d�nd�r
            }
            else
            {
                Debug.Log("havuz yok"); // E�er havuz yoksa mermiyi devre d��� b�rak
            }

        }
    }
    IEnumerator GrowAnimation()
    {
        yield return new WaitForSeconds(3f); // �lk bekleme s�resi
        Vector3 targetScale = originalScale; // Hedef �l�ek orijinal �l�ek
        transform.localScale = Vector3.zero; // Ba�lang��ta �l�ek s�f�r
        Vector3 initialScale = Vector3.zero; // Ba�lang�� �l�e�i
        float elapsedTime = 0f;
        float growthDuration = 5f; // B�y�me s�resi

        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // Son olarak tam hedef �l�e�e ayarla

        if (TryGetComponent(out ResizeableObject component))
        {
            component.TakeDamageAnim();
        }
    }

    IEnumerator DestroyBuilding(string name)
    {
        
        
        // Bina yok olacak i�lemleri burada ger�ekle�tirilebilir
        yield return new WaitForSeconds(1.2f); // �stedi�iniz bir bekleme s�resi ekleyebilirsiniz
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
            float waitTime = 2f; // Sallanma aras�ndaki bekleme s�resi
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator ShakeBuilding()
    {
        yield return new WaitForSeconds(0.3f);
        float shakeDuration = 0.02f; // Sallanma s�resi
        float shakeMagnitude = 0.01f; // Sallanma miktar�

        Vector3 originalPosition = transform.position;

        float elapsedShakeTime = 0f;

        while (elapsedShakeTime < shakeDuration)
        {
            // Rastgele bir sallanma vekt�r� olu�tur
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Sallanm�� pozisyonu ayarla
            transform.position = originalPosition + shakeOffset;

            elapsedShakeTime += Time.deltaTime;
            yield return null;
        }
        
        // Sallanma s�resi bittikten sonra orijinal pozisyona geri d�n
        transform.position = originalPosition;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}