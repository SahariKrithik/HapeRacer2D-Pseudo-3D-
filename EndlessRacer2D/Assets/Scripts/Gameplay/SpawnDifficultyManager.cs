using UnityEngine;

public class SpawnDifficultyManager : MonoBehaviour
{
    [Header("Phase Durations (in seconds)")]
    public float phase1Duration = 30f;
    public float phase2Duration = 30f; // after phase1 ends
    // phase 3 is infinite

    [Header("Scenery Spawn Intervals")]
    public float sceneryPhase1 = 1f;
    public float sceneryPhase2 = 0.75f;
    public float sceneryPhase3 = 0.5f;

    [Header("Hazard/Coin Spawn Intervals")]
    public float hazardPhase1 = 1.5f;
    public float hazardPhase2 = 1f;
    public float hazardPhase3 = 0.8f;

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
                SetHazardInterval(hazardPhase1);
                break;
            case 2:
                SetSceneryInterval(sceneryPhase2);
                SetHazardInterval(hazardPhase2);
                break;
            case 3:
                SetSceneryInterval(sceneryPhase3);
                SetHazardInterval(hazardPhase3);
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

    void SetHazardInterval(float interval)
    {
        if (hazardCoinSpawner != null) hazardCoinSpawner.spawnInterval = interval;
    }
}
