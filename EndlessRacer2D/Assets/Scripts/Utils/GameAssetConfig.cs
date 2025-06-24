using UnityEngine;

[CreateAssetMenu(fileName = "NewGameAssetConfig", menuName = "Game/Game Asset Config")]
public class GameAssetConfig : ScriptableObject
{
    public string assetName;

    public Sprite sprite;

    [Header("Scaling")]
    public float initialScale = 0.2f;
    public float maxScale = 1.0f;
    public float scaleSpeed = 0.5f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Scenery Specific")]
    public bool isScenery = false;
    public bool isRightSide = true; // true = right lane, false = left lane
}
