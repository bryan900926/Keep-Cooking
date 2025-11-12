using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private MoveInput moveInput;
    private Vector2 lastDirection;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnYOffset = 0.1f; // small offset to prevent collider overlap

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveInput = GetComponent<MoveInput>();


        // Ensure player is slightly above initial ground to prevent sticking
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + spawnYOffset, pos.z);

        // Animator setup
        animator.applyRootMotion = false;
    }

    private void FixedUpdate()
    {
        Vector2 movement = moveInput.GetMovementVector().normalized;

        // Update animator parameters
        float speed = movement.magnitude;
        if (speed > 0.1f)
            lastDirection = movement;

        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);
        animator.SetFloat("Speed", speed);

        // Optional sprite flip for horizontal movement
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            transform.localScale = new Vector3(movement.x > 0 ? 1 : -1, 1, 1);
        rb.linearVelocity = movement * moveSpeed;
    }
}
