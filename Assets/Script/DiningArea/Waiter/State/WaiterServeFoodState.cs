using System.Collections.Generic;
using UnityEngine;

public class WaiterServeFoodState : WaiterState
{
    private readonly Queue<OrderInfo> pendingOrders = new();
    private OrderInfo currentOrder;

    public WaiterServeFoodState(WaiterStateManager waiterStateManager, List<OrderInfo> orderInfos)
        : base(waiterStateManager)
    {
        foreach (var order in orderInfos)
        {
            if (order != null)
                pendingOrders.Enqueue(order);
        }
    }

    public override void Enter()
    {
        if (pendingOrders.Count == 0)
        {
            Debug.LogWarning("Waiter has no orders to serve.");
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
            return;
        }

        MoveToNextTable();
    }

    public override void Update()
    {
        if (waiterStateManager.aiPath.reachedDestination && currentOrder != null)
        {
            TryServeDish(currentOrder);
        }

        // If no more dishes, return to Idle
        if (pendingOrders.Count == 0 && waiterStateManager.Holding.HoldingCount == 0)
        {
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
        }
    }

    public override void Exit()
    {
        Debug.Log($"Waiter {waiterStateManager.name} finished serving all dishes.");
        waiterStateManager.Holding.RemoveAllHolding();
        foreach (OrderInfo order in pendingOrders)
        {
            OrderSystem.Instance.AddFailOrder(order, true);
        }

    }

    private void MoveToNextTable()
    {
        if (pendingOrders.Count == 0)
            return;

        currentOrder = pendingOrders.Peek();

        var targetSeat = DiningSystem.Instance.seats[currentOrder.TableIdx].transform;
        waiterStateManager.destinationSetter.target = targetSeat;

        Debug.Log($"Waiter moving to table {currentOrder.TableIdx} with food {currentOrder.FoodIdx}");
    }

    private void TryServeDish(OrderInfo order)
    {
        GameObject customer = DiningSystem.Instance.GetCustomerAtSeat(order.TableIdx);
        if (customer != null)
        {
            var customerState = customer.GetComponent<CustomerStateManager>();
            if (customerState.OrderedFoodIdx == order.FoodIdx)
            {
                Debug.Log($"Waiter served correct dish {order.FoodIdx} to table {order.TableIdx}");
                customerState.ChangeState(new CustomerEatingState(customerState));

                // Remove the served item from waiter’s hands
                var holding = waiterStateManager.Holding;
                GameObject servedItem = holding.HoldingItem.Find(i => i.GetComponent<PickUpV2>().FoodIdx == order.FoodIdx);
                if (servedItem != null)
                    holding.RemoveHoldingItem(servedItem);
                Object.Destroy(servedItem);
            }
            else
            {
                Debug.LogWarning($"Wrong dish! Expected {customerState.OrderedFoodIdx}, got {order.FoodIdx}");
                OrderSystem.Instance.AddFailOrder(order, true);
            }
            pendingOrders.Dequeue();
        }
        else
        {
            Debug.LogWarning($"No customer found at table {order.TableIdx}");
            OrderSystem.Instance.AddFailOrder(order, true);
        }

        // After serving, move to the next table (if any left)
        MoveToNextTable();
    }
}
