
using UnityEngine;

public class ScenerySpawnManager : MonoBehaviour
{
    public ScenerySpawner leftSpawner;
    public ScenerySpawner rightSpawner;

    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (leftSpawner != null)
            leftSpawner.Tick(deltaTime);
        if (rightSpawner != null)
            rightSpawner.Tick(deltaTime);
    }
}
