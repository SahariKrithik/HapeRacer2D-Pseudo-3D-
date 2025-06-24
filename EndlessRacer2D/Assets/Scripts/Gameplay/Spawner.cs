using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public float spawnInterval = 1.5f;

    private float timer;
    private readonly int[] laneIndices = { 0, 1, 2 };

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnHazardsAndCoins();
            timer = 0f;
        }
    }

    void SpawnHazardsAndCoins()
    {
        if (Camera.main == null || LaneManager.Instance == null) return;

        float topY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + 1f;
        List<int> usedLanes = new List<int>();
        List<int> shuffledLanes = new List<int>(laneIndices);
        Shuffle(shuffledLanes);

        // Spawn 1 or 2 obstacles
        int obstacleCount = Random.Range(1, 3);
        for (int i = 0; i < obstacleCount && i < shuffledLanes.Count; i++)
        {
            int lane = shuffledLanes[i];
            float x = LaneManager.Instance.GetLaneX(lane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject obstacle = poolGroup.Get("Obstacle");
            if (obstacle == null) continue;

            obstacle.transform.position = spawnPos;

            var mover = obstacle.GetComponent<MovingObject>();
            mover?.InitPooling(poolGroup, "Obstacle");

            usedLanes.Add(lane);
        }

        // 40% chance to spawn a coin
        if (Random.value < 0.4f)
        {
            List<int> coinLanes = new List<int>(laneIndices);
            foreach (int lane in usedLanes)
                coinLanes.Remove(lane);

            int coinLane = (coinLanes.Count > 0)
                ? coinLanes[Random.Range(0, coinLanes.Count)]
                : laneIndices[Random.Range(0, laneIndices.Length)];

            float x = LaneManager.Instance.GetLaneX(coinLane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject coin = poolGroup.Get("Coin");
            if (coin == null) return;

            coin.transform.position = spawnPos;

            var mover = coin.GetComponent<MovingObject>();
            mover?.InitPooling(poolGroup, "Coin");
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }
}
