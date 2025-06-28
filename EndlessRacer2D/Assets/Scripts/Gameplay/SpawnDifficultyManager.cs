using UnityEngine;

public class SpawnDifficultyManager : MonoBehaviour
{
    [Header("Phase Durations (in seconds)")]
    public float phase1Duration = 30f;
    public float phase2Duration = 30f;

    [Header("Scenery Spawn Intervals")]
    public float sceneryPhase1 = 1f;
    public float sceneryPhase2 = 0.75f;
    public float sceneryPhase3 = 0.5f;

    [Header("Hazard/Coin Shared Spawn Intervals")]
    public float spawnPhase1 = 1.5f;
    public float spawnPhase2 = 1f;
    public float spawnPhase3 = 0.8f;

    [Header("Player Movement Speeds")]
    public float playerSpeedPhase1 = 10f;
    public float playerSpeedPhase2 = 12f;
    public float playerSpeedPhase3 = 14f;

    [Header("Spawner References")]
    public ScenerySpawner leftScenerySpawner;
    public ScenerySpawner rightScenerySpawner;
    public HazardCoinSpawner hazardCoinSpawner;

    private float elapsedTime = 0f;
    private int currentPhase = 1;

    void Start()
    {
        ApplyPhase(1);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (currentPhase == 1 && elapsedTime >= phase1Duration)
        {
            ApplyPhase(2);
        }
        else if (currentPhase == 2 && elapsedTime >= (phase1Duration + phase2Duration))
        {
            ApplyPhase(3);
        }
    }

    void ApplyPhase(int phase)
    {
        currentPhase = phase;

        switch (phase)
        {
            case 1:
                SetSceneryInterval(sceneryPhase1);
                SetSpawnInterval(spawnPhase1);
                SetPlayerSpeed(playerSpeedPhase1);
                break;
            case 2:
                SetSceneryInterval(sceneryPhase2);
                SetSpawnInterval(spawnPhase2);
                SetPlayerSpeed(playerSpeedPhase2);
                break;
            case 3:
                SetSceneryInterval(sceneryPhase3);
                SetSpawnInterval(spawnPhase3);
                SetPlayerSpeed(playerSpeedPhase3);
                break;
        }

        if (hazardCoinSpawner != null)
            hazardCoinSpawner.SetPhase(phase);

        Debug.Log($"Phase {phase} started");
    }

    void SetSceneryInterval(float interval)
    {
        if (leftScenerySpawner != null) leftScenerySpawner.spawnInterval = interval;
        if (rightScenerySpawner != null) rightScenerySpawner.spawnInterval = interval;
    }

    void SetSpawnInterval(float interval)
    {
        if (hazardCoinSpawner != null)
            hazardCoinSpawner.spawnInterval = interval;
    }

    void SetPlayerSpeed(float speed)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.moveSpeed = speed;
        }
    }
}
