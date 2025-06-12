using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentLane = 1; // middle lane

    void Start()
    {
        // Optional: snap to initial position
        UpdatePosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();
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

    private void UpdatePosition()
    {
        float x = LaneManager.Instance.GetLaneX(currentLane);
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
}
