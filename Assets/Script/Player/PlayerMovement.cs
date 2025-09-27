using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, PlayerController.IMoveMentActions
{
    private PlayerController controls;   // Auto-generated input wrapper
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private SpriteRenderer sprite;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerController();
        animator = GetComponent<Animator>();
        controls.MoveMent.SetCallbacks(this); // Hook up callbacks
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        controls.MoveMent.Enable();
    }

    private void OnDisable()
    {
        controls.MoveMent.Disable();
    }
    void Update()
    {
        // Get movement input (from Input System callback or old Input)
        // Example: moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Update Animator parameters
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        if (moveInput.x > 0.1f)
            sprite.flipX = false; // facing right
        else if (moveInput.x < -0.1f)
            sprite.flipX = true;  // facing left
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // === IMoveMentActions Implementation ===
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
