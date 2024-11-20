using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{

    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable;
    private List<GameObject> freeList;
    private List<GameObject> usedList;
    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }
    public GameObject GetObject()
    {
        int totalFree = freeList.Count;
        //Debug.Log("GetObject called, totalFree: " + totalFree);

        if (totalFree == 0 && !expandable)
        {
            Debug.LogWarning("No free bullets and pool is not expandable");
            return null;
        }
        else if (totalFree == 0)
        {
            Debug.Log("No free bullets, generating new one");
            GenerateNewObject();
        }

        GameObject bulletObject = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(bulletObject);
        return bulletObject;
    }
    public void ReturnObject(GameObject obj)
    {
        //Debug.Log("Returning bullet object");
        Debug.Assert(usedList.Contains(obj));
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }
    private void GenerateNewObject()
    {
        GameObject bulletObject = Instantiate(playerBulletPrefab);
        bulletObject.transform.parent = transform;
        bulletObject.SetActive(false);
        freeList.Add(bulletObject);
    }

}
