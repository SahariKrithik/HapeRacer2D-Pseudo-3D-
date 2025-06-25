using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    private int currentLane = 1;
    private float targetX;

    void Start()
    {
        targetX = LaneManager.Instance.GetLaneX(currentLane);
        UpdatePosition(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();

        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = pos;
    }

    public void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            UpdatePosition();
        }
    }

    public void MoveRight()
    {
        if (currentLane < LaneManager.Instance.GetLaneCount() - 1)
        {
            currentLane++;
            UpdatePosition();
        }
    }

    private void UpdatePosition(bool instant = false)
    {
        targetX = LaneManager.Instance.GetLaneX(currentLane);

        if (instant)
        {
            Vector3 pos = transform.position;
            pos.x = targetX;
            transform.position = pos;
        }
    }
}
