using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup; // ✅ Use only ObjectPoolGroup

    public float spawnInterval = 1.5f;
    private float timer;

    private int[] laneIndices = { 0, 1, 2 }; // Matches LaneManager's 3 lanes

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
        float topY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + 1f;

        List<int> usedLanes = new List<int>();
        int obstacleCount = Random.Range(1, 3);
        List<int> shuffledLanes = new List<int>(laneIndices);
        Shuffle(shuffledLanes);

        for (int i = 0; i < obstacleCount && i < shuffledLanes.Count; i++)
        {
            int lane = shuffledLanes[i];
            float x = LaneManager.Instance.GetLaneX(lane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject obj = poolGroup.Get("Obstacle");
            if (obj == null) continue;

            obj.transform.position = spawnPos;

            var mover = obj.GetComponent<MovingObject>();
            if (mover != null)
                mover.InitPooling(poolGroup, "Obstacle");

            obj.SetActive(true);
            usedLanes.Add(lane);
        }

        if (Random.value < 0.4f)
        {
            List<int> availableForCoin = new List<int>(laneIndices);
            foreach (int lane in usedLanes)
                availableForCoin.Remove(lane);

            int coinLane = (availableForCoin.Count > 0)
                ? availableForCoin[Random.Range(0, availableForCoin.Count)]
                : laneIndices[Random.Range(0, laneIndices.Length)];

            float x = LaneManager.Instance.GetLaneX(coinLane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject coin = poolGroup.Get("Coin");
            if (coin == null) return;

            coin.transform.position = spawnPos;

            var mover = coin.GetComponent<MovingObject>();
            if (mover != null)
                mover.InitPooling(poolGroup, "Coin");

            coin.SetActive(true);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
