using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class WaiterServeFoodState : WaiterState
{
    public WaiterServeFoodState(WaiterStateManager waiterStateManager) : base(waiterStateManager)
    {
    }

    public override void Enter()
    {
        if (waiterStateManager.foodIdx != -1 && waiterStateManager.tableIdx != -1)
        {
            Debug.Log($"Waiter is serving food {waiterStateManager.foodIdx} to table {waiterStateManager.tableIdx}.");
            waiterStateManager.destinationSetter.target = waiterStateManager.GetDiningSystem().seats[waiterStateManager.tableIdx].transform;
        }
        else
        {
            Debug.LogWarning("Waiter cannot serve food: foodIdx or tableIdx is invalid.");
            // If indices are invalid, revert to idle state
            waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
        }
    }

    public override void Update()
    {
        if (waiterStateManager.aiPath.reachedDestination)
        {
            CheckIfDishRight();
        }
    }

    public override void Exit()
    {
        waiterStateManager.tableIdx = -1;
        waiterStateManager.foodIdx = -1;
        GameObject dish = waiterStateManager.GetComponent<Holding>().RemoveHolding();
        if (dish != null)
        {
            Object.Destroy(dish);
        }

    }

    private void CheckIfDishRight()
    {
        int tableIdx = waiterStateManager.tableIdx;
        GameObject customer = waiterStateManager.GetDiningSystem().GetCustomerAtSeat(tableIdx);
        if (customer != null && waiterStateManager.foodIdx == customer.GetComponent<CustomerStateManager>().OrderedFoodIdx)
        {
            CustomerStateManager customerStateManager = customer.GetComponent<CustomerStateManager>();
            customerStateManager.ChangeState(new CustomerEatingState(customerStateManager));
        }
        waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
    }
}