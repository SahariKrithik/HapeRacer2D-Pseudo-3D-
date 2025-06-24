using UnityEngine;

public class HazardCoinSpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] hazardConfigs;
    public GameAssetConfig[] coinConfigs;

    public float spawnInterval = 2.5f;
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

        // Always spawn from center-top
        float spawnX = 0f;
        float yStart = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + 1f;
        Vector3 start = new Vector3(spawnX, yStart, 0f);

        // Choose a lane target
        int laneIndex = GetAvailableLane();
        float laneX = LaneManager.Instance.GetLaneX(laneIndex);
        float yEnd = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0)).y - 1f;
        Vector3 end = new Vector3(laneX, yEnd, 0f);

        // Setup moving object
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
