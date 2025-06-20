using UnityEngine;

public class RoadScroller : MonoBehaviour
{
    public Transform[] roadTiles;
    public float scrollSpeed = 3f;

    private float tileHeight;

    void Start()
    {
        if (roadTiles.Length > 0)
            tileHeight = roadTiles[0].GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        foreach (var tile in roadTiles)
        {
            tile.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            if (tile.position.y < -tileHeight)
            {
                float highestY = GetHighestY();
                tile.position = new Vector3(tile.position.x, highestY + tileHeight, tile.position.z);
            }
        }
    }

    float GetHighestY()
    {
        float maxY = float.MinValue;
        foreach (var tile in roadTiles)
        {
            if (tile.position.y > maxY)
                maxY = tile.position.y;
        }
        return maxY;
    }
}
