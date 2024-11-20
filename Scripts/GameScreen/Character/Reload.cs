using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Reload : MonoBehaviour
{
    private InputAction reloadAction;
    public GameObject bullet, trigger;
    public int bulletCount, spareBulletCount, spareBulletCountScreen;
    private Animator animator;
    int reloadAnimation;

    void Start()
    {

        animator = GetComponent<Animator>();
        reloadAnimation = Animator.StringToHash("Reload");
    }

    // Update is called once per frame
    void Update()
    {
        bulletCount = Bullet.bulletCount;
        spareBulletCount = Bullet.spareBulletCount;
        if (spareBulletCount == 0)
        {
            spareBulletCountScreen = 0;
        }
        else
        {
            spareBulletCountScreen = 10 - bulletCount;
        }
        if (bulletCount <= 0)
        {
          //  trigger.GetComponent<AtesEtme>().enabled = false;
            bullet.GetComponent<Bullet>().enabled = false;
            animator.SetBool("Trigger", false);
        }
        else
        {
            //  trigger.GetComponent<AtesEtme>().enabled = false;
            bullet.GetComponent<Bullet>().enabled = true;
            animator.SetBool("Trigger", true);
        }
        if (PlayerController.reloadAction.triggered)
        {
            animator.CrossFade(reloadAnimation, 1.4f);
            if (spareBulletCount < spareBulletCountScreen)
            {
                Bullet.bulletCount += spareBulletCount;
                Bullet.spareBulletCount -= spareBulletCount;
                ActionReload();
            }
            else
            {
                Bullet.bulletCount += spareBulletCountScreen;
                Bullet.spareBulletCount -= spareBulletCountScreen;
                ActionReload();
            }

        }
        StartCoroutine(EnableScript());
    }
    IEnumerator EnableScript()
    {
        yield return new WaitForSeconds(1.1f);
        trigger.SetActive(true);
        animator.SetBool("sarjor", false);
    }

    void ActionReload()
    {
        trigger.SetActive(false);
        //_ses.Play();
    }
}


