using System.Collections.Generic;
using UnityEngine;

public class WaiterServeFoodState : WaiterState
{
    private OrderInfo currentOrder;

    private Holding holding;

    private int orderIndex = 0;

    readonly private List<OrderInfo> orderInfos;

    public WaiterServeFoodState(WaiterStateManager waiterStateManager, List<OrderInfo> orderInfos)
        : base(waiterStateManager)
    {
        this.orderInfos = orderInfos;
    }

    public override void Enter()
    {
        holding = waiterStateManager.Holding;
        if (orderInfos.Count == 0)
        {
            Debug.LogWarning("Waiter has no orders to serve.");
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
            return;
        }

        MoveToNextTable();
    }

    public override void Update()
    {
        // If no more dishes, return to Idle
        if (orderIndex >= orderInfos.Count)
        {
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
        }

        if (orderInfos.Count > 0 && waiterStateManager.aiPath.reachedDestination && currentOrder != null)
        {
            TryServeDish(currentOrder);
        }

    }

    public override void Exit()
    {
        Debug.Log($"Waiter {waiterStateManager.name} finished serving all dishes.");
        waiterStateManager.Holding.RemoveAllHolding();
    }

    private void MoveToNextTable()
    {
        if (orderIndex >= orderInfos.Count)
            return;

        currentOrder = orderInfos[orderIndex];
        var targetSeat = DiningSystem.Instance.seats[currentOrder.TableIdx].transform;
        waiterStateManager.destinationSetter.target = targetSeat;

        Debug.Log($"Waiter moving to table {currentOrder.TableIdx} with food {currentOrder.FoodIdx}");
    }

    private void TryServeDish(OrderInfo order)
    {
        GameObject customer = DiningSystem.Instance.GetCustomerAtSeat(order.TableIdx);
        if (customer != null)
        {
            var customerStateManager = customer.GetComponent<CustomerStateManager>();
            if (customerStateManager.CurrentState is CustomerWaitFoodState)
            {
                if (customerStateManager.OrderedFoodIdx == order.FoodIdx)
                {
                    Debug.Log($"Waiter served correct dish {order.FoodIdx} to table {order.TableIdx}");
                    GameObject servedItem = holding.HoldingItem.Find(i => i.GetComponent<PickUpV2>().FoodIdx == order.FoodIdx);
                    customerStateManager.ChangeState(new CustomerEatingState(customerStateManager, 10));
                    // Remove the served item from waiter’s hands
                    if (servedItem != null)
                        holding.RemoveHoldingItem(servedItem);
                    Object.Destroy(servedItem);
                }
                else
                {
                    Debug.LogWarning($"Wrong dish! Expected {customerStateManager.OrderedFoodIdx}, got {order.FoodIdx}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No customer found at table {order.TableIdx}");
        }
        orderIndex++;
        MoveToNextTable();
    }
}
