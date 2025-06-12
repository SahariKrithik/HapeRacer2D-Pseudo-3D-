using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPool obstaclePool;
    public ObjectPool coinPool;

    public float spawnInterval = 1.5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        float x = LaneManager.Instance.GetRandomLaneX();

        float topY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + 1f;
        Vector3 spawnPos = new Vector3(x, topY, 0);

        bool spawnCoin = Random.value < 0.4f;

        GameObject obj = spawnCoin ? coinPool.GetObject() : obstaclePool.GetObject();
        obj.transform.position = spawnPos;
        obj.transform.localScale = Vector3.one * 0.2f; // Small scale at spawn
        obj.SetActive(true);
    }
}
