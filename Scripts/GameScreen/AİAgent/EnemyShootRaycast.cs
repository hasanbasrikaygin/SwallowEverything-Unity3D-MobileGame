using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class EnemyShootRaycast : MonoBehaviour
{
    [SerializeField] private float bulletDamageForPlayer =50 ;
    [SerializeField] private MuzzleEffectPool muzzleFlashPool;
    [SerializeField] private HitEffectPool hitEffectPool;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject targetPoint;
    //[SerializeField] private GameObject player;
    public GameObject bulletStartPoint;
    [SerializeField] private float effectDuration = 2f;
    // public static DeadCamManager deadCamManager;
    private AudioSource audioSource;
    [SerializeField]  private AudioSource audioSourcePlayer;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
       // deadCamManager = FindObjectOfType<DeadCamManager>();
        if (audioSource == null || muzzleFlashPool == null || hitEffectPool == null || shootSound == null || hitSound == null || targetPoint == null || bulletStartPoint == null)
        {
            Debug.LogError("Bir veya daha fazla bileþen atanmadý!");
            return;
        }
    }

    public void Shoot(Vector3 origin, Vector3 target, AiAgent agent)
    {


        if (agent == null)
        {
            Debug.LogError("AiAgent atanmadý!");
            return;
        }

        Vector3 direction = (target - origin).normalized;
        RaycastHit hit;
        // PlayerHealth.TakeDamage(bulletDamageForPlayer);
        if (Physics.Raycast(origin, direction, out hit))
        {
            if (hit.collider.CompareTag("SensorObj"))
            {

                PlayerHealth.currentHealth -= bulletDamageForPlayer;

            }

            GameObject hitEffectInstance = hitEffectPool.GetEffect();
            if (hitEffectInstance != null)
            {
                hitEffectInstance.transform.position = targetPoint.transform.position;
                hitEffectInstance.transform.rotation = Quaternion.LookRotation(hit.normal);
                hitEffectInstance.SetActive(true);

                StartCoroutine(ReturnBloodHitEffectAfterDuration(hitEffectInstance));
            }

            if (audioSourcePlayer != null)
            {
                audioSourcePlayer.PlayOneShot(hitSound);
            }

            Debug.DrawLine(origin, hit.point, Color.gray, 2.5f);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * 100f, Color.yellow, 2f);
            Debug.Log("Raycast hit nothing");
        }

        GameObject muzzleFlashInstance = muzzleFlashPool.GetEffect();
        if (muzzleFlashInstance != null)
        {
            muzzleFlashInstance.transform.position = origin;
            muzzleFlashInstance.SetActive(true);

            StartCoroutine(ReturnEffectMuzlleAfterDuration(muzzleFlashInstance));
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }


    private IEnumerator ReturnEffectMuzlleAfterDuration(GameObject effectInstance)
    {
        yield return new WaitForSeconds(effectDuration);
        if (effectInstance != null && effectInstance.activeSelf)
        {
            muzzleFlashPool.ReturnEffect(effectInstance);
        }
        else
        {
            Debug.LogWarning("Trying to return a null or inactive muzzle flash instance.");
        }

    }
    private IEnumerator ReturnBloodHitEffectAfterDuration(GameObject effectInstance)
    {
        yield return new WaitForSeconds(effectDuration);
        hitEffectPool.ReturnEffect(effectInstance);
    }
}
