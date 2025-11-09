using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(Animator))]
public class NpcAnimation : MonoBehaviour
{
    private AIPath aiPath;
    private Animator animator;
    private Vector2 lastDirection = Vector2.up;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 velocity = aiPath.velocity;
        float speed = velocity.magnitude;

        // Normalize direction only if moving
        Vector2 direction = speed > 0.01f ? velocity.normalized : lastDirection.normalized;
        animator.SetFloat("Speed", speed);
        // Save last non-zero direction
        if (speed <= 0.1f)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
            transform.localScale = new Vector3(1, 1, 1);
            return;

        }
        // Feed Animator
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        // Flip sprite only if horizontal
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        }
    }
}
