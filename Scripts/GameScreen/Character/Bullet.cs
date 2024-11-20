using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Bullet : MonoBehaviour
{
    public static int bulletCount =0 , spareBulletCount = 0;
    public int ownedBulletCount = 0, ownedSpareBulletCount = 0;
    [SerializeField] TMP_Text textAmmunition, textSpareAmmunition;    // Start is called before the first frame update
    void Awake()
    {
        bulletCount = 0;
        spareBulletCount = 0;
        ownedBulletCount = 0;
        ownedSpareBulletCount = 0;
    }
    void Update()
    {

        ownedBulletCount = bulletCount;
        textAmmunition.text = "" + ownedBulletCount;
        ownedSpareBulletCount = spareBulletCount;
        textSpareAmmunition.text = "" + ownedSpareBulletCount;
    }
}
