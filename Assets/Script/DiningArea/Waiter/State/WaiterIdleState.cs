using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterIdleState : WaiterState
{
    private readonly List<int> targetCounters = new(); // counters to visit
    private Coroutine checkCounterCoroutine;
    private int currentCounterIdx = -1;

    [SerializeField] private readonly float checkInterval = 3f;
    private readonly List<OrderInfo> pendingOrders = new();

    private Holding holding;

    private int idx = 0;

    public WaiterIdleState(WaiterStateManager waiterStateManager) : base(waiterStateManager) { }

    public override void Enter()
    {
        holding = waiterStateManager.GetComponent<Holding>();
        waiterStateManager.FindStandbySpot();
        checkCounterCoroutine = waiterStateManager.StartCoroutine(PeriodicCounterCheck());
    }

    public override void Update()
    {
        // --- Step 2: Move to current counter and pick up food ---
        if (currentCounterIdx != -1)
        {
            float dist = Vector2.Distance(waiterStateManager.transform.position,
                                          CounterManager.Instance.seats[currentCounterIdx].transform.position);
            if (dist < 0.3f && idx < targetCounters.Count)
            {
                var food = CounterManager.Instance.RemoveFoodFromCounter(currentCounterIdx);
                if (food != null)
                    holding.PickUpItem(food);
                else
                    Debug.LogWarning("No food found on counter " + currentCounterIdx);
                idx++;
                MoveToNextCounter(); // increment idx and go to next
            }
        }

        // --- Step 3: Deliver food if done with counters ---
        if (idx >= targetCounters.Count)
        {
            pendingOrders.Clear();
            foreach (var item in holding.HoldingItem)
            {
                if (item.TryGetComponent<PickUpV2>(out var food))
                {
                    var orderInfo = OrderSystem.Instance.GetHighestPriorityOrder(food.FoodIdx);
                    if (orderInfo != null)
                        pendingOrders.Add(orderInfo);
                }
            }

            if (pendingOrders.Count > 0)
            {
                waiterStateManager.ChangeState(new WaiterServeFoodState(waiterStateManager, pendingOrders));
            }
            else
            {
                holding.RemoveAllHolding();
                targetCounters.Clear();
                idx = 0;
                waiterStateManager.FindStandbySpot();
            }
        }
    }

    public override void Exit()
    {
        if (checkCounterCoroutine != null)
            waiterStateManager.StopCoroutine(checkCounterCoroutine);
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
    }

    // step 1: periodically check for available counters with food
    private IEnumerator PeriodicCounterCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (targetCounters.Count == 0 && holding.HasSpace())
            {
                var counters = CounterManager.Instance.FetchFoodsFromCounter(holding.AvailableSpace);
                if (counters.Count > 0)
                {
                    targetCounters.AddRange(counters);
                    idx = 0;
                    MoveToNextCounter();
                }
            }
        }
    }
}
