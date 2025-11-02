using System.Collections.Generic;
using UnityEngine;

public class WaiterIdleState : WaiterState
{
    private List<int> targetCounters = new(); // counters to visit
    private int currentCounterIdx = -1;
    private List<OrderInfo> pendingOrders = new();

    private int idx = 0;

    public WaiterIdleState(WaiterStateManager waiterStateManager) : base(waiterStateManager) { }

    public override void Enter()
    {
        Debug.Log("Waiter " + waiterStateManager.gameObject.GetInstanceID() + " entering Idle State");
        waiterStateManager.FindStandbySpot();
    }

    public override void Update()
    {
        var holding = waiterStateManager.Holding;

        // --- Step 1: if not full, keep fetching food from counters ---
        if (targetCounters.Count == 0 && holding.HasSpace() && CounterManager.Instance.GetFoodCountOnCounter > 0)
        {
            List<int> currentCounterIdxs = CounterManager.Instance.FetchFoodsFromCounter(holding.AvailableSpace);
            targetCounters.AddRange(currentCounterIdxs);
            MoveToNextCounter();
        }
        if (currentCounterIdx != -1 && Vector2.Distance(waiterStateManager.transform.position, CounterManager.Instance.seats[currentCounterIdx].transform.position) < 0.3f)
        {
            waiterStateManager.Holding.PickUpItem(CounterManager.Instance.RemoveFoodFromCounter(currentCounterIdx));
        }
        if (idx < targetCounters.Count) {
            MoveToNextCounter();
        }
        if (idx >= targetCounters.Count)
        {
            foreach (var item in holding.HoldingItem)
            {
                var food = item.GetComponent<PickUpV2>();
                if (food == null) continue;

                OrderInfo orderInfo = OrderSystem.Instance.GetHighestPriorityOrder(food.FoodIdx);
                if (orderInfo != null)
                {
                    pendingOrders.Add(orderInfo);
                }
            }

            if (pendingOrders.Count > 0)
            {
                waiterStateManager.ChangeState(new WaiterServeFoodState(waiterStateManager, pendingOrders));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Waiter " + waiterStateManager.gameObject.GetInstanceID() + " exiting Idle State");
        waiterStateManager.ClearStandbySpot();
    }

    private void MoveToNextCounter()
    {
        if (idx >= targetCounters.Count)
        {
            return;
        }
        currentCounterIdx = targetCounters[idx];
        waiterStateManager.destinationSetter.target = CounterManager.Instance.seats[currentCounterIdx].transform;
        idx++;
    }
}
