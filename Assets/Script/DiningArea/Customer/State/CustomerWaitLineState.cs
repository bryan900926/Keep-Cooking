public class CustomerWaitLineState : CustomerState
{
    public CustomerWaitLineState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Update()
    {
        if (customerStateManager.LiningIdx != -1 && customerStateManager.AiPath.reachedDestination)
        {
            TryToDine(customerStateManager);
        }
        TryToWaitLine(customerStateManager);
    }

    private void TryToWaitLine(CustomerStateManager customer)
    {
        if (customer.LiningIdx != -1) return; // Already in line
        int idx = customer.QueueSystem.FetchAvailSeat();
        if (idx != -1)
        {
            customer.LiningIdx = idx;
            customer.DestinationSetter.target = customer.QueueSystem.seats[idx].transform;
        }
    }
    private void TryToDine(CustomerStateManager customer)
    {
        if (customer.DiningIdx != -1 ) return; // Already dining
        int idx = customer.DiningSystem.FetchAvailSeat();
        if (idx != -1)
        {
            customer.DiningIdx = idx;
            customer.QueueSystem.FreeSeat(customer.LiningIdx);
            customer.DiningSystem.SeatToCustomer[customer.DiningIdx] = customer.gameObject;
            customer.ChangeState(new CustomerWaitFoodState(customer));
        }
    }
}