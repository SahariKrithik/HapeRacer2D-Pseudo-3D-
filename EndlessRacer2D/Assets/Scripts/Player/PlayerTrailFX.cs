using UnityEngine;

public class PlayerTrailFX : MonoBehaviour
{
    public GameObject trailFXLeft;
    public GameObject trailFXRight;

    [Header("Random Trail Flicker")]
    public float minToggleTime = 0.3f;
    public float maxToggleTime = 0.6f;

    [Header("Movement Sensitivity")]
    public float movementThreshold = 0.05f;

    private float toggleTimer;
    private float nextToggleTime;
    private Vector3 lastPosition;
    private bool overrideByInput = false;

    void Start()
    {
        lastPosition = transform.position;
        ScheduleNextToggle();
    }

    void Update()
    {
        HandleMovementOverride();
        if (!overrideByInput)
        {
            toggleTimer += Time.deltaTime;
            if (toggleTimer >= nextToggleTime)
            {
                ToggleBothTrails();
                ScheduleNextToggle();
            }
        }
        lastPosition = transform.position;
    }

    void HandleMovementOverride()
    {
        float deltaX = transform.position.x - lastPosition.x;

        if (Mathf.Abs(deltaX) > movementThreshold)
        {
            overrideByInput = true;

            if (deltaX < 0f)
            {
                // Moving Left
                EnableTrail(trailFXLeft);
                DisableTrail(trailFXRight);
            }
            else
            {
                // Moving Right
                EnableTrail(trailFXRight);
                DisableTrail(trailFXLeft);
            }
        }
        else
        {
            overrideByInput = false;
        }
    }

    void ScheduleNextToggle()
    {
        toggleTimer = 0f;
        nextToggleTime = Random.Range(minToggleTime, maxToggleTime);
    }

    void ToggleBothTrails()
    {
        bool enable = Random.value > 0.5f;
        trailFXLeft?.SetActive(enable);
        trailFXRight?.SetActive(enable);
    }

    void EnableTrail(GameObject trail)
    {
        if (trail != null && !trail.activeSelf)
            trail.SetActive(true);
    }

    void DisableTrail(GameObject trail)
    {
        if (trail != null && trail.activeSelf)
            trail.SetActive(false);
    }
}
