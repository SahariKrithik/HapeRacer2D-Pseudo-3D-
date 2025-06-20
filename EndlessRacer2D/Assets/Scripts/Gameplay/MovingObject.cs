using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float scaleSpeed = 0.5f;

    [Header("Obstacle Sprite Config")]
    public HazardSpriteConfig[] hazardConfigs;

    [Header("Coin Sprite Config")]
    public HazardSpriteConfig[] coinConfigs;

    private SpriteRenderer spriteRenderer;

    private float destroyY = -6f;
    private float maxScale = 1.2f;
    private float currentScale = 0.1f;

    void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Determine if it's a coin or obstacle
        if (CompareTag("Obstacle") && hazardConfigs != null && hazardConfigs.Length > 0)
        {
            int index = Random.Range(0, hazardConfigs.Length);
            var config = hazardConfigs[index];

            spriteRenderer.sprite = config.sprite;
            currentScale = config.initialScale;
            maxScale = config.maxScale;
        }
        else if (CompareTag("Coin") && coinConfigs != null && coinConfigs.Length > 0)
        {
            int index = Random.Range(0, coinConfigs.Length);
            var config = coinConfigs[index];

            spriteRenderer.sprite = config.sprite;
            currentScale = config.initialScale;
            maxScale = config.maxScale;
        }
        else
        {
            currentScale = 0.2f;
            maxScale = 1.0f;
        }

        transform.localScale = Vector3.one * currentScale;
    }

    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            currentScale = Mathf.Min(currentScale, maxScale);
            transform.localScale = Vector3.one * currentScale;
        }

        if (transform.position.y < destroyY)
        {
            ObjectPool pool = GetComponentInParent<ObjectPool>();
            if (pool != null)
                pool.ReturnToPool(gameObject);
            else
                gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("Coin"))
            {
                var scoreManager = FindObjectOfType<ScoreManager>();
                scoreManager.AddPoints(50);
            }
            else if (CompareTag("Obstacle"))
            {
                GameManager.Instance.GameOver();
            }

            gameObject.SetActive(false);
        }
    }
}
