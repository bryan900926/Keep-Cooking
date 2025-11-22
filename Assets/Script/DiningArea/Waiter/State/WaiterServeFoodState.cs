using System.Collections.Generic;
using UnityEngine;

public class WaiterServeFoodState : WaiterState
{
    private readonly Queue<OrderInfo> pendingOrders = new();
    private OrderInfo currentOrder;

    private Holding holding;

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
        holding = waiterStateManager.Holding;
        Debug.Log($"Waiter {waiterStateManager.name} starts serving food.");
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
        // If no more dishes, return to Idle
        if (pendingOrders.Count == 0)
        {
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
        }

        if (pendingOrders.Count > 0 && waiterStateManager.aiPath.reachedDestination && currentOrder != null)
        {
            TryServeDish(currentOrder);
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
            var customerStateManager = customer.GetComponent<CustomerStateManager>();
            if (customerStateManager.CurrentState is not CustomerWaitFoodState)
            {
                Debug.Log(customerStateManager.CurrentState.GetType().Name);
                return;
            }
            if (customerStateManager.OrderedFoodIdx == order.FoodIdx)
            {
                Debug.Log($"Waiter served correct dish {order.FoodIdx} to table {order.TableIdx}");
                GameObject servedItem = holding.HoldingItem.Find(i => i.GetComponent<PickUpV2>().FoodIdx == order.FoodIdx);
                float freshness = servedItem.GetComponent<DishProperty>().Freshness;
                customerStateManager.ChangeState(new CustomerEatingState(customerStateManager, freshness));
                // Remove the served item from waiter’s hands
                if (servedItem != null)
                    holding.RemoveHoldingItem(servedItem);
                Object.Destroy(servedItem);
            }
            else
            {
                Debug.LogWarning($"Wrong dish! Expected {customerStateManager.OrderedFoodIdx}, got {order.FoodIdx}");
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
