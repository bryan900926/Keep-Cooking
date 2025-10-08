using UnityEngine;

public class WaiterIdleState : WaiterState
{
    public WaiterIdleState(WaiterStateManager waiterStateManager) : base(waiterStateManager)
    {
    }

    public override void Enter()
    {
        waiterStateManager.destinationSetter.target = waiterStateManager.GetWaitingSpot();
    }

    public override void Update()
    {
        if (waiterStateManager.foodIdx != -1 && waiterStateManager.tableIdx != -1)
        {
            waiterStateManager.ChangeState(new WaiterServeFoodState(waiterStateManager));
        }

    }

    public override void Exit()
    {
    }
}
