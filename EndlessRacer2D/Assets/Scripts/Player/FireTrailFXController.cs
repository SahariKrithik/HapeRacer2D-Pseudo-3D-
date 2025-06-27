using UnityEngine;
using System.Collections;

public class FireTrailFXController : MonoBehaviour
{
    [Header("Fire Trail References")]
    public GameObject fireTrailLeft;
    public GameObject fireTrailRight;

    [Header("Spawn Timing")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;

    [Header("Fade Settings")]
    public float fadeDuration = 0.4f;

    [Header("Flicker Settings")]
    [Range(0f, 1f)]
    public float flickerChance = 0.5f; // 50% chance to flicker before fade
    public int flickerCount = 4;
    public float flickerSpeed = 0.05f;

    private SpriteRenderer leftSR;
    private SpriteRenderer rightSR;

    private void Start()
    {
        if (fireTrailLeft != null)
            leftSR = fireTrailLeft.GetComponent<SpriteRenderer>();
        if (fireTrailRight != null)
            rightSR = fireTrailRight.GetComponent<SpriteRenderer>();

        StartCoroutine(FireTrailRoutine());
    }

    private IEnumerator FireTrailRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            EnableTrail(fireTrailLeft, leftSR);
            EnableTrail(fireTrailRight, rightSR);

            yield return new WaitForSeconds(0.2f); // Optional delay before effect

            bool doFlicker = Random.value < flickerChance;

            if (doFlicker)
            {
                if (fireTrailLeft.activeSelf)
                    StartCoroutine(FlickerThenFade(fireTrailLeft, leftSR));
                if (fireTrailRight.activeSelf)
                    StartCoroutine(FlickerThenFade(fireTrailRight, rightSR));
            }
            else
            {
                if (fireTrailLeft.activeSelf)
                    StartCoroutine(FadeAndDisable(fireTrailLeft, leftSR));
                if (fireTrailRight.activeSelf)
                    StartCoroutine(FadeAndDisable(fireTrailRight, rightSR));
            }
        }
    }

    private void EnableTrail(GameObject obj, SpriteRenderer sr)
    {
        if (obj != null && sr != null)
        {
            obj.SetActive(true);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }
    }

    private IEnumerator FlickerThenFade(GameObject obj, SpriteRenderer sr)
    {
        for (int i = 0; i < flickerCount; i++)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(flickerSpeed);
            obj.SetActive(true);
            yield return new WaitForSeconds(flickerSpeed);
        }

        yield return StartCoroutine(FadeAndDisable(obj, sr));
    }

    private IEnumerator FadeAndDisable(GameObject obj, SpriteRenderer sr)
    {
        float t = 0f;
        Color original = sr.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(original.a, 0f, t / fadeDuration);
            sr.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }

        obj.SetActive(false);
    }
}
