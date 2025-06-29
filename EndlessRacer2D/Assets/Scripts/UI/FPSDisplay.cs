using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float timer;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= 0.5f)
        {
            int fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            fpsText.text = $"FPS: {fps}";
            timer = 0f;
        }
    }
}
