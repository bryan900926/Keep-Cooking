public class CustomerWaitLineState : CustomerState
{
    public CustomerWaitLineState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Update()
    {
        if (customerStateManager.DiningIdx == -1 && customerStateManager.LiningIdx != -1)
        {
            if (TryToReceipt(customerStateManager))
            {
                return;
            }
        }
        TryToWaitLine(customerStateManager);
    }

    private void TryToWaitLine(CustomerStateManager customer)
    {
        if (customer.LiningIdx != -1) return;
        int idx = customer.Qs.FetchAvailSeat();
        if (idx != -1)
        {
            customer.LiningIdx = idx;
            customer.DestinationSetter.target = customer.Qs.seats[idx].transform;
        }
    }

    private bool TryToReceipt(CustomerStateManager customer)
    {
        if (customer.DiningIdx != -1) return false;

        int idx = ReceiptSystem.Instance.FetchAvailSeat();
        if (idx != -1)
        {
            customer.Qs.FreeSeat(customer.LiningIdx);
            customer.LiningIdx = -1;
            customer.ChangeState(new CustomerOrderState(customer, idx));

            return true;
        }
        return false;
    }
}