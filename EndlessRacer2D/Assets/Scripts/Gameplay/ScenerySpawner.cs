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
        var validConfigs = System.Array.FindAll(sceneryConfigs,
            config => config.isScenery && config.isRightSide == isRightSideSpawner);
        if (validConfigs.Length == 0) return;

        var config = validConfigs[Random.Range(0, validConfigs.Length)];
        GameObject obj = poolGroup.Get(config.assetName);
        if (obj == null) return;

        var mover = obj.GetComponent<MovingObject>();
        if (mover != null)
        {
            mover.SetCustomSpeed(config.moveSpeed, config.scaleSpeed, config.initialScale, config.maxScale);

            obj.transform.localScale = Vector3.one * config.initialScale;
            Debug.Log($"[ScenerySpawner] Forced scale reset before enabling: {obj.transform.localScale}");

            Vector3 adjustedTarget = targetPoint.position;
            adjustedTarget.x += isRightSideSpawner ? 3.5f : -3.5f;

            mover.SetPath(spawnPoint.position, adjustedTarget, true);
            mover.InitPooling(poolGroup, config.assetName);
        }

        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = config.sprite;

        // ✅ Activation now happens after all setup
        obj.SetActive(true);
    }
}
