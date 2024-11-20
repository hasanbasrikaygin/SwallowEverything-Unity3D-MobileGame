using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable;
    private List<GameObject> freeList;
    private List<GameObject> usedList;
    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();
        for(int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }
    public GameObject GetObject() { 
        int totalFree =freeList.Count;
        if(totalFree == 0 && !expandable) return null;
        else if (totalFree == 0) GenerateNewObject();

        GameObject bulletObject = freeList[totalFree -1];
        freeList.RemoveAt(totalFree-1);
        usedList.Add(bulletObject);
        return bulletObject;
    }
    public void ReturnObject(GameObject obj)
    {
        Debug.Assert(usedList.Contains(obj));
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }
    private void GenerateNewObject()
    {
        GameObject bulletObject = Instantiate(bulletPrefab);
        bulletObject.transform.parent = transform;
        bulletObject.SetActive(false);
        freeList.Add(bulletObject);
    }

}
