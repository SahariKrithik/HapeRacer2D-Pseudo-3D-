using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarFXController : MonoBehaviour
{
    [Header("Trail FX References")]
    public GameObject trailFXLeft;
    public GameObject trailFXRight;

    [Header("Trail Flicker Settings")]
    public float trailFlickerMinTime = 0.3f;
    public float trailFlickerMaxTime = 0.7f;
    public float trailFadeDuration = 0.3f;

    [Header("Ghost Trail Settings")]
    public GameObject ghostPrefab; // Prefab with only SpriteRenderer
    public float ghostSpawnInterval = 0.15f;
    public float ghostFadeDuration = 1f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.4f);
    public int ghostPoolSize = 10;

    private float ghostTimer;
    private SpriteRenderer leftTrailRenderer;
    private SpriteRenderer rightTrailRenderer;

    private Queue<GameObject> ghostPool = new Queue<GameObject>();

    private void Start()
    {
        if (trailFXLeft != null)
            leftTrailRenderer = trailFXLeft.GetComponent<SpriteRenderer>();
        if (trailFXRight != null)
            rightTrailRenderer = trailFXRight.GetComponent<SpriteRenderer>();

        StartCoroutine(RandomTrailFlicker());
        InitializeGhostPool();
    }

    private void Update()
    {
        HandleInputTrailOverride();
        HandleGhostSpawn();
    }

    private void HandleInputTrailOverride()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0)
        {
            EnableTrail(trailFXLeft);
            DisableTrail(trailFXRight);
        }
        else if (horizontal > 0)
        {
            EnableTrail(trailFXRight);
            DisableTrail(trailFXLeft);
        }
    }

    private void HandleGhostSpawn()
    {
        ghostTimer += Time.deltaTime;
        if (ghostTimer >= ghostSpawnInterval)
        {
            SpawnGhost();
            ghostTimer = 0f;
        }
    }

    private IEnumerator RandomTrailFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(trailFlickerMinTime, trailFlickerMaxTime));

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal == 0)
            {
                bool enableLeft = Random.value > 0.5f;
                bool enableRight = Random.value > 0.5f;

                if (enableLeft) EnableTrail(trailFXLeft);
                else FadeAndDisable(trailFXLeft, leftTrailRenderer);

                if (enableRight) EnableTrail(trailFXRight);
                else FadeAndDisable(trailFXRight, rightTrailRenderer);
            }
        }
    }

    private void EnableTrail(GameObject trail)
    {
        if (trail != null && !trail.activeSelf)
        {
            var sr = trail.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
            trail.SetActive(true);
        }
    }

    private void DisableTrail(GameObject trail)
    {
        if (trail != null && trail.activeSelf)
            StartCoroutine(FadeAndDisable(trail, trail.GetComponent<SpriteRenderer>()));
    }

    private IEnumerator FadeAndDisable(GameObject trail, SpriteRenderer sr)
    {
        if (trail == null || sr == null) yield break;

        float t = 0f;
        Color original = sr.color;
        while (t < trailFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(original.a, 0f, t / trailFadeDuration);
            sr.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        trail.SetActive(false);
    }

    private void InitializeGhostPool()
    {
        for (int i = 0; i < ghostPoolSize; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab);
            ghost.SetActive(false);
            ghostPool.Enqueue(ghost);
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab == null || ghostPool.Count == 0) return;

        GameObject ghost = ghostPool.Dequeue();
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        SpriteRenderer carSR = GetComponent<SpriteRenderer>();

        if (ghostSR != null && carSR != null)
        {
            ghostSR.sprite = carSR.sprite;
            ghostSR.sortingOrder = carSR.sortingOrder - 1;
            ghostSR.color = ghostColor;
        }

        ghost.SetActive(true);
        StartCoroutine(FadeAndRecycleGhost(ghostSR, ghost));
    }

    private IEnumerator FadeAndRecycleGhost(SpriteRenderer sr, GameObject obj)
    {
        float t = 0f;
        Color original = ghostColor;

        while (t < ghostFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(original.a, 0f, t / ghostFadeDuration);
            if (sr != null)
                sr.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        obj.SetActive(false);
        ghostPool.Enqueue(obj);
    }
}
