using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float destroyY = -6f;

    private float maxScale;
    private float currentScale;
    private float scaleSpeed;
    private float moveSpeed;

    private float startX = 0f;
    private float targetX;

    private bool isScenery = false;

    void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite selectedSprite = null;
        GameAssetConfig config = null;

        isScenery = false;

        if (CompareTag("Obstacle"))
        {
            var hazardList = GameSettings.Instance.hazardConfigs;
            int index = Random.Range(0, hazardList.Count);
            config = hazardList[index];
        }
        else if (CompareTag("Coin"))
        {
            var coinList = GameSettings.Instance.coinConfigs;
            int index = Random.Range(0, coinList.Count);
            config = coinList[index];
        }

        if (config != null)
        {
            selectedSprite = config.sprite;
            isScenery = config.isScenery;
            spriteRenderer.sprite = selectedSprite;

            currentScale = config.initialScale;
            maxScale = config.maxScale;
            scaleSpeed = config.scaleSpeed;
            moveSpeed = config.moveSpeed;
        }

        transform.localScale = Vector3.one * currentScale;
        startX = 0f;
        targetX = transform.position.x;

        // Apply lane shift only to obstacles/coins, not scenery
        if (!isScenery)
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            currentScale = Mathf.Min(currentScale, maxScale);
        }

        if (!isScenery)
        {
            float t = Mathf.InverseLerp(0.2f, maxScale, currentScale);
            float newX = Mathf.Lerp(startX, targetX, t);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        transform.localScale = Vector3.one * currentScale;

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
        if (!other.CompareTag("Player")) return;

        if (CompareTag("Coin"))
            FindObjectOfType<ScoreManager>().AddPoints(50);
        else if (CompareTag("Obstacle"))
            GameManager.Instance.GameOver();

        gameObject.SetActive(false);
    }

    public void SetCustomSpeed(float move, float scale, float initial, float max)
    {
        moveSpeed = move;
        scaleSpeed = scale;
        currentScale = initial;
        maxScale = max;
        transform.localScale = Vector3.one * initial;
    }
}
