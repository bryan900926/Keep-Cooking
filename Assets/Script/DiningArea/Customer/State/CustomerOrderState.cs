using UnityEngine;

public class CustomerOrderState : CustomerState
{
    private GameObject receipt;
    private int receiptIdx;
    private bool ifDine = false;

    private bool beenToReceipt = false;
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
        if (IsReachedReceipt() && !beenToReceipt)
        {
            GetOrderedFood();
        }
        if (ifDine && customerStateManager.DiningIdx == -1)
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
        if (ifDine)
        {
            OrderSystem.Instance.AddNewOrder(customerStateManager.OrderedFoodIdx, customerStateManager.DiningIdx, customerStateManager.Energy.SurviveTime, customerStateManager.gameObject);
        }
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
        customerStateManager.OrderedFoodIdx = Menu.Instance.RandomSpawnForCustomer(customerStateManager.gameObject, customerStateManager.trans);
        customerStateManager.sellprice = PriceEditor.Instance.GetSellingPrice(customerStateManager.OrderedFoodIdx);
        float desiredPrice = customerStateManager.BuyingPrice[customerStateManager.OrderedFoodIdx];

        float sellingPrice = customerStateManager.sellprice;
        float upperbound = customerStateManager.customerProperty.uppertruevalue[customerStateManager.OrderedFoodIdx];
        float lowerbound = customerStateManager.customerProperty.lowertruevalue[customerStateManager.OrderedFoodIdx];
        if (desiredPrice >= sellingPrice)
        {
            ifDine = true;
            if (sellingPrice <= lowerbound)
            {
                customerStateManager.ReactGreat();
                CustomerPropertyManager.Instance.Addsatisfactory(customerStateManager.customerProperty, 2);
            }
            else
            {
                customerStateManager.ReactGood();
                CustomerPropertyManager.Instance.Addsatisfactory(customerStateManager.customerProperty, 1);
            }
            CustomerPropertyManager.Instance.NiceCustomer += 1;
        }
        else
        {
            if (sellingPrice >= upperbound)
            {
                customerStateManager.ReactTerrible();
                CustomerPropertyManager.Instance.Addsatisfactory(customerStateManager.customerProperty, -2);
            }
            else
            {
                customerStateManager.ReactBad();
                CustomerPropertyManager.Instance.Addsatisfactory(customerStateManager.customerProperty, -1);
            }
            CustomerPropertyManager.Instance.BadCustomer += 1;

            ReputationSystem.Instance.DecreaseReputation(customerStateManager.Minusreputation);
            CustomerPropertyManager.Instance.Updateprop(customerStateManager.customerProperty);
            customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
        }
        beenToReceipt = true;
    }
}