using UnityEngine;
public class WaiterIdleState : WaiterState
{
    private int counterIdx = -1;



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
        if (counterIdx == -1 && waiterStateManager.foodIdx == -1)
        {
            FindCounter();
        }
        else if (counterIdx != -1 && Vector2.Distance(waiterStateManager.transform.position, CounterManager.Instance.seats[counterIdx].transform.position) < 0.5f)
        {
            CounterManager.Instance.RemoveFoodFromCounter(counterIdx, waiterStateManager);
            counterIdx = -1;
        }
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

    private void FindCounter()
    {
        counterIdx = CounterManager.Instance.FetchFoodFromCounter();
        if (counterIdx != -1)
        {
            waiterStateManager.destinationSetter.target = CounterManager.Instance.seats[counterIdx].transform;
        }
    }
}
