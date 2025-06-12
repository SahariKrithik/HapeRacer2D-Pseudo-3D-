using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float scaleSpeed = 0.5f;

    private float destroyY = -6f;

    void Update()
    {
        // Move downward
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // Gradually scale up
        transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;

        // Off-screen? Return to pool
        if (transform.position.y < destroyY)
        {
            ObjectPool pool = GetComponentInParent<ObjectPool>(); // safest for later
            if (pool != null)
                pool.ReturnToPool(gameObject);
            else
                gameObject.SetActive(false); // fallback
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Coin"))
            {
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                scoreManager.AddPoints(50);
            }
            else if (gameObject.CompareTag("Obstacle"))
            {
                // Trigger game over
                GameManager.Instance.GameOver();
            }

            gameObject.SetActive(false); // Return to pool
        }
    }


}
