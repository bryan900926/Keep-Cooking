using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private MoveInput moveInput;
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 lastDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = GetComponent<MoveInput>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = moveInput.GetMovementVector();

        float speed = direction.magnitude;

        if (speed > 0.1f)
        {
            lastDirection = direction.normalized;
            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        }
        else
        {
            // Keep facing last direction
            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        }
        animator.SetFloat("Speed", speed);
        // Optional sprite flip (for left/right)
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
    }
}
