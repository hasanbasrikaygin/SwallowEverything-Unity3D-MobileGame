using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpBooster : MonoBehaviour
{
    //private float originalPlayerGarvity = -9.81f;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    [SerializeField] private float boostDuration = 5f;
    public AudioManager audioManager;
    [SerializeField] ParticleSystem BuffEffect;
    private bool isEffectRunning = false;
    void Start()
    {
        isEffectRunning = false;
        if (audioManager == null)
        {
            Debug.LogError("audioManager bileþen atanmadý!");
            return;
        }
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log( gameObject);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(EnableEffectForDuration(2f));
            Transform firstChild = transform.GetChild(0);
            Transform firstGrandChild = GetFirstChild(firstChild);
            firstGrandChild.GetComponent<MeshRenderer>().enabled = false;
            audioManager.TakeJumpBoosterAudioSource();
            if (!playerController.isJumpBoosted)
            {
                StartCoroutine(GravityBoost());
            }
            else
            {
                // Hýzlandýrýcýyý tekrar aldýðýnda süreyi uzat
                playerController.jumpBoostTimer += boostDuration;
            }
        }
    }
    IEnumerator GravityBoost()
    {
        playerController.isJumpBoosted = true;
        playerController.jumpBoostTimer = Time.time + boostDuration;

        playerController.jumpHeight = 4f;


        while (Time.time < playerController.jumpBoostTimer)
        {
            yield return null;
        }

        playerController.isJumpBoosted = false;
        playerController.jumpHeight = 1;
    }
    Transform GetFirstChild(Transform parent)
    {
        if (parent.childCount > 0)
        {
            return parent.GetChild(0);
        }
        else
        {
            Debug.Log("No grandchildren found for the parent.");
            return null;
        }
    }
    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap

        BuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        BuffEffect.Stop(); // Partikül sistemi durdurulur
        //this.gameObject.SetActive(false);
        isEffectRunning = false; // Efekt bittiðinde kontrol deðiþkenini false yap
    }
}
