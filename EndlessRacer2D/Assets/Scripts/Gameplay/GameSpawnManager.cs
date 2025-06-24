using UnityEngine;

public class GameSpawnManager : MonoBehaviour
{
    public ScenerySpawnManager scenerySpawnManager;
    public HazardCoinSpawner hazardCoinSpawner;

    void Update()
    {
        float dt = Time.deltaTime;

        if (scenerySpawnManager != null)
            scenerySpawnManager.Tick(dt);

        if (hazardCoinSpawner != null)
            hazardCoinSpawner.Tick(dt);
    }
}
