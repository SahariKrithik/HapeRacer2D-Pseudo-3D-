using UnityEngine;
using System.Collections.Generic;

public class LaneDividerPool : MonoBehaviour
{
    [Header("Pooling")]
    public GameObject linePrefab;
    public int poolSize = 10;
    public float spacing = 0.45f;
    public float scrollSpeed = 2f;

    [Header("Scaling")]
    public Vector2 startScale = new Vector2(0.05f, 0.026f);
    public Vector2 endScale = new Vector2(0.07f, 0.042f);
    [Range(0.1f, 5f)] public float scaleLerpSpeed = 1f;

    private List<GameObject> linePool;
    private Dictionary<GameObject, float> initialY = new Dictionary<GameObject, float>();

    private float spawnY;
    private float despawnY;

    void Start()
    {
        Vector3 top = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));
        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f));

        spawnY = top.y + 0.2f;
        despawnY = bottom.y - 0.2f;

        InitializePool();
    }

    void InitializePool()
    {
        linePool = new List<GameObject>();
        initialY.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPos = new Vector3(0f, spawnY - i * spacing, 0f);
            GameObject line = Instantiate(linePrefab, spawnPos, Quaternion.identity, transform);
            linePool.Add(line);
            initialY[line] = spawnPos.y;  // Track original Y position
        }
    }

    void Update()
    {
        foreach (GameObject line in linePool)
        {
            if (line == null) continue;

            line.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            // Dynamic scaling logic
            float currentY = line.transform.position.y;
            float startY = initialY[line];
            float t = Mathf.InverseLerp(startY, despawnY, currentY);
            t = Mathf.Clamp01(Mathf.Pow(t, scaleLerpSpeed));

            Vector2 newScale = Vector2.Lerp(startScale, endScale, t);
            line.transform.localScale = new Vector3(newScale.x, newScale.y, 1f);

            // Recycle when off screen
            if (currentY < despawnY)
            {
                float maxY = GetHighestYInPool();
                Vector3 resetPos = new Vector3(line.transform.position.x, maxY + spacing, line.transform.position.z);
                line.transform.position = resetPos;
                initialY[line] = resetPos.y;  // Update tracked Y
            }
        }
    }

    float GetHighestYInPool()
    {
        float maxY = float.MinValue;
        foreach (GameObject line in linePool)
        {
            if (line != null && line.transform.position.y > maxY)
                maxY = line.transform.position.y;
        }
        return maxY;
    }
}
