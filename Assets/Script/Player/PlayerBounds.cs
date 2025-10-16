using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public BoxCollider2D cameraBounds; // Assign the same collider used by Cinemachine Confiner
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (cameraBounds == null) return;

        Vector3 pos = transform.position;
        Bounds bounds = cameraBounds.bounds;

        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);

        transform.position = pos;
    }
}
