using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    private float destroyDistance = 12f;

    private float maxScale;
    private float initialScale;
    private float currentScale;
    private float scaleSpeed;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isScenery = false;

    private string poolId;
    private ObjectPoolGroup poolGroup;

    private float moveProgress = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    void OnEnable()
    {
        transform.localScale = Vector3.one * initialScale;
        currentScale = initialScale;
        moveProgress = 0f;
        transform.position = startPos; // fjgnkjfg
    }

    void Update()
    {
        // Grow scale over time
        if (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            currentScale = Mathf.Min(currentScale, maxScale);
        }

        transform.localScale = Vector3.one * currentScale;

        // Movement based on interpolation progress
        moveProgress += Time.deltaTime * 0.5f; // fixed speed for non-scenery
        transform.position = Vector3.Lerp(startPos, endPos, moveProgress);

        if (moveProgress >= 1f)
        {
            ReturnToPool();
        }

        if (transform.position.y < mainCam.transform.position.y - destroyDistance)
        {
            ReturnToPool();
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CompareTag("Coin"))
        {
            FindObjectOfType<ScoreManager>()?.AddPoints(50);
        }
        else if (CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }

        ReturnToPool();
    }

    public void SetCustomSpeed(float move, float scale, float initial, float max)
    {
        scaleSpeed = scale;
        initialScale = initial;
        maxScale = max;
    }

    public void InitPooling(ObjectPoolGroup group, string id)
    {
        poolGroup = group;
        poolId = id;
    }

    public void SetPath(Vector3 from, Vector3 to, bool isSceneryObj)
    {
        startPos = from;
        endPos = to;
        isScenery = isSceneryObj;
        transform.position = from;
    }

    private void ReturnToPool()
    {
        if (poolGroup != null && !string.IsNullOrEmpty(poolId))
            poolGroup.Return(poolId, gameObject);
        else
            gameObject.SetActive(false);
    }
}