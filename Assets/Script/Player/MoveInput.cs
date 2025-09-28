using UnityEngine;

public class MoveInput : MonoBehaviour
{
    private PlayerMovementInput playerMovementInput;
    void Awake()
    {
        playerMovementInput = new PlayerMovementInput();
        playerMovementInput.Enable();

    }

    public Vector2 GetMovementVector()
    {
        Vector2 input = playerMovementInput.Player.Move.ReadValue<Vector2>();
        input = input.normalized;
        return input;
    }
}
