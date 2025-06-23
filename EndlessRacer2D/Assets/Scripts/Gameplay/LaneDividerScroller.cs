using UnityEngine;

public class LaneDividerScroller : MonoBehaviour
{
    public float scrollSpeed = 3f;
    public float resetY = -6f;     // Adjust based on off-screen threshold
    public float startY = 6f;      // Adjust based on spawn height

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        if (transform.position.y < resetY)
        {
            transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        }
    }
}
