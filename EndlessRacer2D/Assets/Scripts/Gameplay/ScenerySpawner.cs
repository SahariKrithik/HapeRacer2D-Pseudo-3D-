using UnityEngine;

public class ScenerySpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] sceneryConfigs;
    public bool isRightSideSpawner = true;
    public float spawnInterval = 2f;

    public Transform spawnPoint;
    public Transform targetPoint;

    private float timer;
    private GameAssetConfig lastUsedConfig;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnScenery();
            timer = 0f;
        }
    }

    void SpawnScenery()
    {
        // Filter configs for this side
        var validConfigs = System.Array.FindAll(
            sceneryConfigs,
            config => config.isScenery && config.isRightSide == isRightSideSpawner
        );
        if (validConfigs.Length == 0) return;

        // Filter out the last used one (if more than 1 option exists)
        GameAssetConfig[] filtered = validConfigs;
        if (validConfigs.Length > 1 && lastUsedConfig != null)
        {
            filtered = System.Array.FindAll(validConfigs, c => c != lastUsedConfig);
        }

        // Choose a new config
        var config = filtered[Random.Range(0, filtered.Length)];
        lastUsedConfig = config;

        GameObject obj = poolGroup.Get(config.assetName);
        if (obj == null) return;

        var mover = obj.GetComponent<MovingObject>();
        if (mover != null)
        {
            mover.SetCustomSpeed(config.moveSpeed, config.scaleSpeed, config.initialScale, config.maxScale);
            obj.transform.localScale = Vector3.one * config.initialScale;

            Vector3 adjustedTarget = targetPoint.position;
            adjustedTarget.x += isRightSideSpawner ? 3.5f : -3.5f;

            mover.SetPath(spawnPoint.position, adjustedTarget, true);
            mover.InitPooling(poolGroup, config.assetName);
        }

        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = config.sprite;

        obj.SetActive(true);
    }
}
