using UnityEngine;

public class HazardCoinSpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] hazardConfigs;
    public GameAssetConfig[] coinConfigs;

    public float spawnInterval = 2.5f;
    public float spawnYOffset = -2.25f;

    private float timer;
    private int lastUsedLane = -1;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnHazardOrCoin();
            timer = 0f;
        }
    }

    void SpawnHazardOrCoin()
    {
        bool spawnCoin = Random.value > 0.5f;
        var configList = spawnCoin ? coinConfigs : hazardConfigs;
        if (configList == null || configList.Length == 0) return;

        var config = configList[Random.Range(0, configList.Length)];
        GameObject obj = poolGroup.Get(config.assetName);
        if (obj == null) return;

        // Get lane index and target X
        int laneIndex = GetAvailableLane();
        float laneX = LaneManager.Instance.GetLaneX(laneIndex);

        // Explicit offset based on lane
        float offsetX = 0f;
        if (laneIndex == 0) offsetX = -0.3f;     // Left
        else if (laneIndex == 1) offsetX = 0f;   // Center
        else if (laneIndex == 2) offsetX = 0.3f; // Right

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
        lastUsedLane = laneIndex;
    }

    int GetAvailableLane()
    {
        int laneCount = LaneManager.Instance.GetLaneCount();
        int lane;
        do
        {
            lane = Random.Range(0, laneCount);
        } while (lane == lastUsedLane && laneCount > 1);
        return lane;
    }
}
