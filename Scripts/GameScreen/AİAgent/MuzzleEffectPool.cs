using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffectPool : MonoBehaviour
{
    [SerializeField] private GameObject muzzleFlashPrefab;
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
            GenerateNewEffect();
        }
    }

    public GameObject GetEffect()
    {
        int totalFree = freeList.Count;

        if (totalFree == 0 && !expandable)
            return null;
        else if (totalFree == 0)
            GenerateNewEffect();

        GameObject effectObject = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(effectObject);

        return effectObject;
    }

    public void ReturnEffect(GameObject obj)
    {
        Debug.Assert(usedList.Contains(obj));

        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }

    private void GenerateNewEffect()
    {
        GameObject effectObject = Instantiate(muzzleFlashPrefab);
        effectObject.transform.parent = transform;
        effectObject.SetActive(false);
        freeList.Add(effectObject);
  
    }
}
