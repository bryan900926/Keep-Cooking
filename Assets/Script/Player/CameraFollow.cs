using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;     // Player (or object to follow)
    [SerializeField] private float smoothSpeed = 0.15f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
