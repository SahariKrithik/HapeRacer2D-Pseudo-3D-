using UnityEngine;

public class RoadScroller : MonoBehaviour
{
    public Transform[] roadTiles;
    public float scrollSpeed = 3f;

    private float tileHeight;

    void Start()
    {
        tileHeight = roadTiles[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        foreach (Transform tile in roadTiles)
        {
            tile.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            // When tile goes off-screen (below), reposition above
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
        foreach (Transform tile in roadTiles)
        {
            if (tile.position.y > maxY)
                maxY = tile.position.y;
        }
        return maxY;
    }
}
