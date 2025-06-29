using UnityEngine;
using System.Collections.Generic;

public class HazardCoinSpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] hazardConfigs;
    public GameAssetConfig[] coinConfigs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2.0f;
    public float spawnYOffset = -2.25f;
    public float endYOffset = -2.5f;

    [Header("End Lane X Offsets")]
    public float endOffsetXLeft = -1f;
    public float endOffsetXRight = 1f;

    [Header("Advanced Spawning")]
    public bool enableDoubleHazardInPhase2And3 = true;
    [Range(0f, 1f)]
    public float doubleHazardChance = 0.3f;

    private float timer = 0f;
    private int lastUsedLane = -1;
    private int currentPhase = 1;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnHazardsAndMaybeCoin();
            timer = 0f;
        }
    }

    public void SetPhase(int phase)
    {
        currentPhase = phase;
    }

    void SpawnHazardsAndMaybeCoin()
    {
        List<int> occupiedLanes = new List<int>();
        int laneIndex = GetAvailableLane(-1);
        SpawnSingle(hazardConfigs, laneIndex);
        occupiedLanes.Add(laneIndex);

        if (enableDoubleHazardInPhase2And3 && currentPhase >= 2 && Random.value < doubleHazardChance)
        {
            int secondLane = GetAvailableLane(laneIndex);
            if (secondLane != laneIndex)
            {
                SpawnSingle(hazardConfigs, secondLane);
                occupiedLanes.Add(secondLane);
            }
        }

        if (Random.value < 0.4f && coinConfigs != null && coinConfigs.Length > 0)
        {
            int coinLane = GetAvailableLane(-1, occupiedLanes);
            if (coinLane != -1)
            {
                SpawnSingle(coinConfigs, coinLane);
            }
        }

        lastUsedLane = laneIndex;
    }

    void SpawnSingle(GameAssetConfig[] configList, int laneIndex)
    {
        var config = configList[Random.Range(0, configList.Length)];
        GameObject obj = poolGroup.Get(config.assetName);
        if (obj == null) return;

        float offsetX = laneIndex == 0 ? -0.3f : laneIndex == 2 ? 0.3f : 0f;
        float laneX = LaneManager.Instance.GetLaneX(laneIndex);

        float yStart = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + spawnYOffset;
        float jitter = Random.Range(-0.3f, 0.3f);
        float yEnd = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0)).y + endYOffset + jitter;

        float endX = laneX;
        if (laneIndex == 0) endX += endOffsetXLeft;
        else if (laneIndex == 2) endX += endOffsetXRight;

        Vector3 start = new Vector3(offsetX, yStart, 0f);
        Vector3 end = new Vector3(endX, yEnd, 0f);

        var mover = obj.GetComponent<MovingObject>();
        if (mover != null)
        {
            mover.SetCustomSpeed(config.moveSpeed, config.scaleSpeed, config.initialScale, config.maxScale);

            // ✅ Ensure scale is correct before activation
            obj.transform.localScale = Vector3.one * config.initialScale;
            //Debug.Log($"[HazardCoinSpawner] Forced scale reset before enabling: {obj.transform.localScale}");

            mover.SetPath(start, end, false);
            mover.InitPooling(poolGroup, config.assetName);
            mover.SetResetScaleOnReturn(endYOffset <= -0.5f);
        }

        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = config.sprite;

        // ✅ Now safe to enable after setup
        obj.SetActive(true);
    }

    int GetAvailableLane(int excludeLane, List<int> excludeList = null)
    {
        int laneCount = LaneManager.Instance.GetLaneCount();
        List<int> possibleLanes = new List<int>();

        for (int i = 0; i < laneCount; i++)
        {
            if (i == excludeLane) continue;
            if (excludeList != null && excludeList.Contains(i)) continue;
            possibleLanes.Add(i);
        }

        if (possibleLanes.Count == 0)
            return -1;

        return possibleLanes[Random.Range(0, possibleLanes.Count)];
    }

    int GetAvailableLane(int excludeLane)
    {
        return GetAvailableLane(excludeLane, null);
    }
}
