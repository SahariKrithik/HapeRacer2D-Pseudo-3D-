using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    public bool allowExpansion = true;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        WarmUpPool();
    }

    public void WarmUpPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        if (allowExpansion)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }

        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
