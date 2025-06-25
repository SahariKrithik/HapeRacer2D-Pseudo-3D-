using UnityEngine;

public class GhostFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float fadeSpeed = 2f;
    private Color initialColor;
    private bool fading = false;
    private System.Action<GhostFX> onFadeComplete;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialColor = sr.color;
    }

    public void Init(Sprite sprite, Vector3 position, float fadeSpeed, System.Action<GhostFX> onComplete)
    {
        transform.position = position;
        sr.sprite = sprite;
        this.fadeSpeed = fadeSpeed;
        sr.color = initialColor;
        fading = true;
        onFadeComplete = onComplete;
    }

    void Update()
    {
        if (!fading) return;

        Color c = sr.color;
        c.a -= Time.deltaTime * fadeSpeed;
        sr.color = c;

        if (c.a <= 0f)
        {
            fading = false;
            onFadeComplete?.Invoke(this);
        }
    }
}
