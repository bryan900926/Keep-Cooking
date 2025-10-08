
using UnityEngine;

public class CustomerWaitLineState : CustomerState
{
    public CustomerWaitLineState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Update()
    {
        if (customerStateManager.LiningIdx != -1 && customerStateManager.aiPath.reachedDestination)
        {
            TryToDine(customerStateManager);
        }
        TryToWaitLine(customerStateManager);
    }

    private void TryToWaitLine(CustomerStateManager customer)
    {
        if (customer.LiningIdx != -1) return; // Already in line
        int idx = customer.queueSystem.FetchAvailSeat();
        if (idx != -1)
        {
            customer.LiningIdx = idx;
            customer.destinationSetter.target = customer.queueSystem.seats[idx].transform;
        }
    }
    private void TryToDine(CustomerStateManager customer)
    {
        if (customer.DiningIdx != -1 ) return; // Already dining
        int idx = customer.diningSystem.FetchAvailSeat();
        if (idx != -1)
        {
            customer.DiningIdx = idx;
            customer.queueSystem.FreeSeat(customer.LiningIdx);
            customer.diningSystem.SeatToCustomer[customer.DiningIdx] = customer.gameObject;
            customer.ChangeState(new CustomerWaitFoodState(customer));
        }
    }
}