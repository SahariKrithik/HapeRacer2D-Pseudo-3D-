
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolEntry
{
    public string id;
    public GameObject prefab;
    public int poolSize = 5;
    public bool warmUp = true;
    public bool expandable = true;

    [HideInInspector] public Queue<GameObject> pool = new Queue<GameObject>();
    [HideInInspector] public Transform parent;
    private bool initialized = false;

    public void Initialize(Transform root)
    {
        if (initialized) return;
        initialized = true;

        parent = new GameObject($"Pool_{id}").transform;
        parent.SetParent(root);

        if (warmUp)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab, parent);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }
}

public class ObjectPoolGroup : MonoBehaviour
{
    public List<PoolEntry> pools = new List<PoolEntry>();
    private Dictionary<string, PoolEntry> lookup = new Dictionary<string, PoolEntry>();

    void Awake()
    {
        foreach (var entry in pools)
        {
            entry.Initialize(transform);
            lookup[entry.id] = entry;
        }
    }

    public GameObject Get(string id)
    {
        if (!lookup.ContainsKey(id)) return null;

        var entry = lookup[id];

        if (entry.pool.Count > 0)
        {
            var obj = entry.pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        if (entry.expandable)
        {
            GameObject obj = Instantiate(entry.prefab, entry.parent);
            obj.SetActive(true);
            return obj;
        }

        return null;
    }

    public void Return(string id, GameObject obj)
    {
        if (!lookup.ContainsKey(id)) return;

        obj.SetActive(false);
        lookup[id].pool.Enqueue(obj);
    }
}
