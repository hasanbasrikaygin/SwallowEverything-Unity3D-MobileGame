using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
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
       


        if (obj != null && usedList.Contains(obj))

        {
            Debug.Assert(usedList.Contains(obj));
            obj.SetActive(false);
            usedList.Remove(obj);
            freeList.Add(obj);
        }
        else
        {
            Debug.LogWarning("Trying to return a null or unused effect instance.");
        }
    }

    private void GenerateNewEffect()
    {
        GameObject effectObject = Instantiate(hitEffectPrefab);
        effectObject.transform.parent = transform;
        effectObject.SetActive(false);
        freeList.Add(effectObject);
    }
}
