using UnityEngine;

public class CustomerLeaveState : CustomerState
{
    private const string EXIT_TAG = "Exit";
    public CustomerLeaveState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Enter()
    {
        Transform exitPoint = GameObject.FindGameObjectWithTag(EXIT_TAG).transform;
        customerStateManager.DestinationSetter.target = exitPoint;
    }

    public override void Update()
    {
        if (customerStateManager.AiPath.reachedDestination)
        {
            customerStateManager.ViewEffect.ApplyEffect();
            Object.Destroy(customerStateManager.gameObject);
        }

    }
}