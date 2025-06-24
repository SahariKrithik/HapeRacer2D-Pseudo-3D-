// GameSettings.cs
using UnityEngine;
using System.Collections.Generic;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Header("Hazards")]
    public List<GameAssetConfig> hazardConfigs;

    [Header("Coins")]
    public List<GameAssetConfig> coinConfigs;

    [Header("Scenery")]
    public List<GameAssetConfig> sceneryConfigs;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameAssetConfig GetHazardConfig(Sprite sprite) =>
        hazardConfigs.Find(c => c.sprite == sprite);

    public GameAssetConfig GetCoinConfig(Sprite sprite) =>
        coinConfigs.Find(c => c.sprite == sprite);

    public GameAssetConfig GetSceneryConfig(Sprite sprite) =>
        sceneryConfigs.Find(c => c.sprite == sprite);
}
