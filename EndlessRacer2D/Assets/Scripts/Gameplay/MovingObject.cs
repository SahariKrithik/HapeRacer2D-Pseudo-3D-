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
    private bool resetScaleOnReturn = false;

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
        transform.position = startPos;

        //Debug.Log($"[OnEnable] {gameObject.name} initial scale set to: {transform.localScale}");
    }

    void OnDisable()
    {
        //Debug.Log($"[OnDisable] {gameObject.name} scale before disabling: {transform.localScale}");
        transform.localScale = Vector3.zero;
        moveProgress = 0f;
    }

    void Update()
    {
        if (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            currentScale = Mathf.Min(currentScale, maxScale);
        }

        transform.localScale = Vector3.one * currentScale;

        moveProgress += Time.deltaTime * 0.5f;
        transform.position = Vector3.Lerp(startPos, endPos, moveProgress);

        if (moveProgress >= 1f || transform.position.y < mainCam.transform.position.y - destroyDistance)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CompareTag("Coin"))
        {
            FindObjectOfType<ScoreManager>()?.AddPoints(40);
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

    public float GetInitialScale() => initialScale;

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

    public void SetResetScaleOnReturn(bool shouldReset)
    {
        resetScaleOnReturn = shouldReset;
    }

    private void ReturnToPool()
    {
        if (resetScaleOnReturn)
        {
            currentScale = initialScale;
            transform.localScale = Vector3.one * initialScale;
           // Debug.Log($"[ReturnToPool] {gameObject.name} scale reset to: {transform.localScale}");
        }

        if (poolGroup != null && !string.IsNullOrEmpty(poolId))
            poolGroup.Return(poolId, gameObject);
        else
            gameObject.SetActive(false);
    }
}
