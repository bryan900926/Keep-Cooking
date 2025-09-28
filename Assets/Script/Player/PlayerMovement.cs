using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private MoveInput moveInput;


    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveInput = GetComponent<MoveInput>();
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.GetMovementVector() * moveSpeed * Time.fixedDeltaTime);
    }

}
