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

    public void Initialize(Transform root, ObjectPoolGroup group)
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
                InitObject(obj, group, id);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }

    public static void InitObject(GameObject obj, ObjectPoolGroup group, string id)
    {
        var moving = obj.GetComponent<MovingObject>();
        if (moving != null)
        {
            moving.InitPooling(group, id);
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
            entry.Initialize(transform, this);
            lookup[entry.id] = entry;
        }
    }

    public GameObject Get(string id)
    {
        if (!lookup.ContainsKey(id)) return null;

        var entry = lookup[id];
        GameObject obj = null;

        if (entry.pool.Count > 0)
        {
            obj = entry.pool.Dequeue();
        }
        else if (entry.expandable)
        {
            obj = Instantiate(entry.prefab, entry.parent);
            PoolEntry.InitObject(obj, this, id);
        }

        if (obj != null)
        {
            // Safe redundant init for safety in case prefab was not warmed up
            PoolEntry.InitObject(obj, this, id);
            obj.SetActive(true);
        }

        return obj;
    }

    public void Return(string id, GameObject obj)
    {
        if (!lookup.ContainsKey(id)) return;

        obj.SetActive(false);
        lookup[id].pool.Enqueue(obj);
    }
}
