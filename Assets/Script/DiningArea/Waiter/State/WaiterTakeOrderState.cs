using UnityEngine;

public class WaiterTakeOrderState : WaiterState
{
    public WaiterTakeOrderState(WaiterStateManager waiterStateManager) : base(waiterStateManager)
    {
    }

    public override void Enter()
    {
        // Logic to move to the customer and take order
        // This is a placeholder; actual implementation would involve pathfinding and animations
        Debug.Log("Waiter is taking order from customer.");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Waiter has taken the order and is now serving food.");
    }
}
