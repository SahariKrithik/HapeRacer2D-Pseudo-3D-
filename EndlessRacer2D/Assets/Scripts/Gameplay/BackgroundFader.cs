using System.Collections;
using UnityEngine;

public class BackgroundFader : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundPhase
    {
        public SpriteRenderer renderer;
        public float holdDuration = 10f; // Each background stays visible for 10s
    }

    [Header("Phases (order: Morning -> Afternoon -> Night)")]
    public BackgroundPhase[] phases;

    [Header("Fade Duration (seconds)")]
    public float fadeDuration = 2f;

    private int currentIndex = 0;

    void Start()
    {
        if (phases == null || phases.Length == 0)
        {
            Debug.LogError("BackgroundFader: No phases assigned!");
            return;
        }

        foreach (var phase in phases)
        {
            if (phase.renderer == null)
            {
                Debug.LogError("BackgroundFader: One or more SpriteRenderers are missing.");
                return;
            }
        }
        // Set only the first sprite fully visible, others invisible
        for (int i = 0; i < phases.Length; i++)
        {
            SetAlpha(phases[i].renderer, i == 0 ? 1f : 0f);
        }

        StartCoroutine(FadeCycle());
    }

    IEnumerator FadeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(phases[currentIndex].holdDuration);

            int nextIndex = (currentIndex + 1) % phases.Length;
            yield return StartCoroutine(FadeToNext(phases[currentIndex].renderer, phases[nextIndex].renderer));

            currentIndex = nextIndex;
        }
    }

    IEnumerator FadeToNext(SpriteRenderer from, SpriteRenderer to)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            SetAlpha(from, 1f - t);
            SetAlpha(to, t);
            timer += Time.deltaTime;
            yield return null;
        }

        SetAlpha(from, 0f);
        SetAlpha(to, 1f);
    }

    void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        if (renderer == null) return;
        Color c = renderer.color;
        c.a = alpha;
        renderer.color = c;
    }
}
