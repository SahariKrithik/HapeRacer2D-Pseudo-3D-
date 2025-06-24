using UnityEngine;

public class ScenerySpawnManager : MonoBehaviour
{
    public ScenerySpawner leftSpawner;
    public ScenerySpawner rightSpawner;

    public void Tick(float deltaTime)
    {
        if (leftSpawner != null)
            leftSpawner.Tick(deltaTime);
        if (rightSpawner != null)
            rightSpawner.Tick(deltaTime);
    }
}
