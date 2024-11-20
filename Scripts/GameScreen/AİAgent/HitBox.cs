using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    
    private float enemyBulletDamage;

    public PlayerBulletPool pool;

    private void Awake()
    {
        enemyBulletDamage = PlayerPrefs.GetFloat("Damage",20f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBullet")
        {
            //Debug.Log("PlayerBulletPool mermisi ");
            health.TakeDamage(enemyBulletDamage, Vector3.zero);
            //pool.ReturnObject(other.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
