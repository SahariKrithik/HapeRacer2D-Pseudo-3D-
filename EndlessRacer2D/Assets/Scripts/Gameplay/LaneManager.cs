using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    public float[] lanePositions = { -4f, 0f, 4f }; // Set in Inspector

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public float GetLaneX(int laneIndex)
    {
        return lanePositions[Mathf.Clamp(laneIndex, 0, lanePositions.Length - 1)];
    }

    public float GetRandomLaneX()
    {
        int index = Random.Range(0, lanePositions.Length);
        return GetLaneX(index);
    }

    public int GetLaneCount()
    {
        return lanePositions.Length;
    }
}
