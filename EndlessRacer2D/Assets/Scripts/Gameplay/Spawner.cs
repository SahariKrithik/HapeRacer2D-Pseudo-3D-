using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public ObjectPool obstaclePool;
    public ObjectPool coinPool;

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

        // Track used lanes to avoid overlap
        List<int> usedLanes = new List<int>();

        // Spawn up to 2 obstacles in random lanes
        int obstacleCount = Random.Range(1, 3); // 1 or 2 obstacles
        List<int> shuffledLanes = new List<int>(laneIndices);
        Shuffle(shuffledLanes);

        for (int i = 0; i < obstacleCount && i < shuffledLanes.Count; i++)
        {
            int lane = shuffledLanes[i];
            float x = LaneManager.Instance.GetLaneX(lane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject obj = obstaclePool.GetObject();
            obj.transform.position = spawnPos;
            obj.SetActive(true);

            usedLanes.Add(lane);
        }

        // Try to spawn coin
        if (Random.value < 0.4f)
        {
            List<int> availableForCoin = new List<int>(laneIndices);
            foreach (int lane in usedLanes)
                availableForCoin.Remove(lane);

            int coinLane;
            if (availableForCoin.Count > 0)
            {
                // Prefer an unused lane
                coinLane = availableForCoin[Random.Range(0, availableForCoin.Count)];
            }
            else
            {
                // All lanes used by obstacles — allow overlap by choosing any lane
                coinLane = laneIndices[Random.Range(0, laneIndices.Length)];
            }

            float x = LaneManager.Instance.GetLaneX(coinLane);
            Vector3 spawnPos = new Vector3(x, topY, 0);

            GameObject coin = coinPool.GetObject();
            coin.transform.position = spawnPos;
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
