using UnityEngine;

/// <summary>
/// Simple helper that forwards a trigger into the door animator whenever a new customer spawns.
/// </summary>
public class DoorController : MonoBehaviour
{
    public static DoorController Instance { get; private set; }
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openTriggerName = "Open";
    [SerializeField] private float reopenCooldown = 0.25f;

    private float lastTriggerTime = -1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (doorAnimator == null)
            doorAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called by the spawner right before a customer prefab is instantiated.
    /// </summary>
    public void TriggerDoorOpen()
    {
        if (doorAnimator == null || string.IsNullOrEmpty(openTriggerName))
            return;

        if (lastTriggerTime >= 0f && Time.time - lastTriggerTime < reopenCooldown)
            return;

        doorAnimator.ResetTrigger(openTriggerName);
        doorAnimator.SetTrigger(openTriggerName);
        lastTriggerTime = Time.time;
    }
}
