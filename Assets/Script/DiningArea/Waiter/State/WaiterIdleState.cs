using UnityEngine;
public class WaiterIdleState : WaiterState
{
    public WaiterIdleState(WaiterStateManager waiterStateManager) : base(waiterStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Waiter " + waiterStateManager.gameObject.GetInstanceID() + " entering Idle State");
        waiterStateManager.FindStandbySpot();
    }

    public override void Update()
    {
        if (waiterStateManager.foodIdx != -1)
        {
            OrderInfo orderInfo = OrderSystem.Instance.GetHighestPriorityOrder(waiterStateManager.foodIdx);
            if (orderInfo != null)
            {
                waiterStateManager.tableIdx = orderInfo.TableIdx;
                waiterStateManager.ChangeState(new WaiterServeFoodState(waiterStateManager, orderInfo));
            }
        }

    }

    public override void Exit()
    {
        Debug.Log("Waiter " + waiterStateManager.gameObject.GetInstanceID() + " exiting Idle State");
        waiterStateManager.ClearStandbySpot();

    }
}
