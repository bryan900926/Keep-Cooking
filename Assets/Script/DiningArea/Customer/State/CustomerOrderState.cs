using UnityEngine;

public class CustomerOrderState : CustomerState
{
    private GameObject receipt;
    private int receiptIdx;
    private bool ifDine;
    public CustomerOrderState(CustomerStateManager customerStateManager, int receiptIdx) : base(customerStateManager)
    {
        this.receipt = ReceiptSystem.Instance.seats[receiptIdx];
        this.receiptIdx = receiptIdx;
    }

    public override void Enter()
    {
        customerStateManager.DestinationSetter.target = receipt.transform;
    }

    public override void Update()
    {
        if (IsReachedReceipt() && !ifDine)
        {
            GetOrderedFood();
        }
        if (ifDine)
        {
            TryToDine(customerStateManager);
            if (customerStateManager.DiningIdx != -1)
            {
                customerStateManager.ChangeState(new CustomerWaitFoodState(customerStateManager));
            }
        }
    }

    public override void Exit()
    {
        ReceiptSystem.Instance.FreeSeat(receiptIdx);
        OrderSystem.Instance.AddNewOrder(customerStateManager.OrderedFoodIdx, customerStateManager.DiningIdx, customerStateManager.Energy.SurviveTime, customerStateManager.gameObject);
    }

    private void TryToDine(CustomerStateManager customer)
    {
        if (customer.DiningIdx != -1) return; // Already dining
        int idx = DiningSystem.Instance.FetchAvailSeat();
        if (idx != -1)
        {
            customer.DiningIdx = idx;
            DiningSystem.Instance.SeatToCustomer[customer.DiningIdx] = customer.gameObject;
        }
    }

    private bool IsReachedReceipt()
    {
        float distance = Vector3.Distance(customerStateManager.transform.position, receipt.transform.position);
        return distance < 1.0f;
    }

    private void GetOrderedFood()
    {
        customerStateManager.OrderedFoodIdx = Menu.Instance.RandomSpawnForCustomer(customerStateManager.gameObject);
        float desiredPrice = PriceEditor.Instance.GetPriceForCustomer(customerStateManager.OrderedFoodIdx);
        float sellingPrice = PriceEditor.Instance.GetSellingPrice(customerStateManager.OrderedFoodIdx);
        if (desiredPrice > sellingPrice)
        {
            customerStateManager.BuyingPrice = sellingPrice;
            ifDine = true;
        }
        else
        {
            customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
        }
    }
}