using UnityEngine;

public class ScenerySpawner : MonoBehaviour
{
    public ObjectPoolGroup poolGroup;
    public GameAssetConfig[] sceneryConfigs;
    public bool isRightSideSpawner = true;
    public float spawnInterval = 2f;

    public Transform spawnPoint;  // SquareLeft or SquareRight
    public Transform targetPoint; // CircleLeft or CircleRight

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

            Vector3 adjustedTarget = targetPoint.position;

            // Push it away from the road further
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
