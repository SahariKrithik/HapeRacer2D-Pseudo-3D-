using UnityEngine;

public class HazardCoinSpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] hazardConfigs;
    public GameAssetConfig[] coinConfigs;

    public float spawnInterval = 2.5f;
    public float spawnYOffset = -2.25f;

    [Header("Advanced Spawning")]
    public bool enableDoubleHazardInPhase2And3 = true;
    [Range(0f, 1f)]
    public float doubleHazardChance = 0.3f;

    private float timer;
    private int lastUsedLane = -1;
    private int currentPhase = 1;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnHazardOrCoin();
            timer = 0f;
        }
    }

    public void SetPhase(int phase)
    {
        currentPhase = phase;
    }

    void SpawnHazardOrCoin()
    {
        bool spawnCoin = Random.value > 0.5f;
        var configList = spawnCoin ? coinConfigs : hazardConfigs;
        if (configList == null || configList.Length == 0) return;

        int laneIndex = GetAvailableLane(-1);
        SpawnSingle(configList, laneIndex);

        // Optional second hazard in phase 2/3
        if (!spawnCoin && enableDoubleHazardInPhase2And3 && currentPhase >= 2 && Random.value < doubleHazardChance)
        {
            int secondLane = GetAvailableLane(laneIndex);
            if (secondLane != laneIndex)
                SpawnSingle(configList, secondLane);
        }

        lastUsedLane = laneIndex;
    }

    void SpawnSingle(GameAssetConfig[] configList, int laneIndex)
    {
        var config = configList[Random.Range(0, configList.Length)];
        GameObject obj = poolGroup.Get(config.assetName);
        if (obj == null) return;

        // Offset per lane
        float offsetX = laneIndex == 0 ? -0.3f : laneIndex == 2 ? 0.3f : 0f;
        float laneX = LaneManager.Instance.GetLaneX(laneIndex);

        float yStart = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + spawnYOffset;
        float yEnd = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0)).y - 1f;

        Vector3 start = new Vector3(offsetX, yStart, 0f);
        Vector3 end = new Vector3(laneX, yEnd, 0f);

        var mover = obj.GetComponent<MovingObject>();
        if (mover != null)
        {
            mover.SetCustomSpeed(config.moveSpeed, config.scaleSpeed, config.initialScale, config.maxScale);
            mover.SetPath(start, end, false);
            mover.InitPooling(poolGroup, config.assetName);
        }

        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = config.sprite;

        obj.SetActive(true);
    }

    int GetAvailableLane(int excludeLane)
    {
        int laneCount = LaneManager.Instance.GetLaneCount();
        int lane;
        do
        {
            lane = Random.Range(0, laneCount);
        } while ((lane == lastUsedLane || lane == excludeLane) && laneCount > 1);
        return lane;
    }
}
