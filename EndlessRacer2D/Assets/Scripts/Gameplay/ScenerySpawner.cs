using UnityEngine;

public class ScenerySpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;                  // 🟢 Use ObjectPoolGroup
    public GameAssetConfig[] sceneryConfigs;
    public bool isRightSideSpawner = true;
    public float spawnInterval = 2f;

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

        GameObject obj = poolGroup.Get(config.assetName);       // 🟢 Use assetName as pool ID
        if (obj == null) return;

        obj.transform.position = GetSpawnPosition(isRightSideSpawner);
        obj.transform.localScale = Vector3.one * config.initialScale;

        var spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.sprite = config.sprite;

        var mover = obj.GetComponent<MovingObject>();
        if (mover != null)
        {
            mover.SetCustomSpeed(config.moveSpeed, config.scaleSpeed,
                                 config.initialScale, config.maxScale);
        }

        obj.SetActive(true);
    }

    Vector3 GetSpawnPosition(bool isRight)
    {
        float x = isRight ? 5.5f : -5.5f;
        float y = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + 1f;
        return new Vector3(x, y, 0f);
    }
}
