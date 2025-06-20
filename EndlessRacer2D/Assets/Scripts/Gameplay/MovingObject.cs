using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float scaleSpeed = 0.5f;

    [Header("Optional")]
    public Sprite[] hazardSprites; // assign in prefab
    private SpriteRenderer spriteRenderer;

    private float destroyY = -6f;

    void OnEnable()
    {
        // Reset scale in case object is reused
        transform.localScale = Vector3.one * 0.1f;

        // Randomize sprite if it's a hazard
        if (CompareTag("Obstacle") && hazardSprites != null && hazardSprites.Length > 0)
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = hazardSprites[Random.Range(0, hazardSprites.Length)];
        }
    }

    void Update()
    {
        // Move downward
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // Scale up to simulate depth
        transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;

        // Off-screen? Return to pool
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
