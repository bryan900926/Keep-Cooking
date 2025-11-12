using UnityEngine;

public class CustomerEatingState : CustomerState
{
    private float eatingDuration = 5f; // Duration for eating in seconds
    private float elapsedTime = 0f;

    public CustomerEatingState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Enter()
    {
        if (customerStateManager.GetComponent<Holding>().HoldingItem.Count == 0)
        {
            Debug.LogError("Customer has no food to eat!");
        }
        GameObject holdingFood = customerStateManager.GetComponent<Holding>().HoldingItem[0];
        if (holdingFood != null)
        {
            holdingFood.GetComponent<DishStateManager>().ChangeState(new DishEatenState(holdingFood.GetComponent<DishStateManager>(), eatingDuration));
        }
        elapsedTime = 0f;
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= eatingDuration)
        {
            // Finished eating, transition to leaving state
            customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
        }
    }

    public override void Exit()
    {
        DiningSystem.Instance.FreeSeat(customerStateManager.DiningIdx);
        customerStateManager.DiningIdx = -1;
        customerStateManager.GetComponent<Holding>().RemoveAllHolding();
        ScoreManager.Instance.AddRevenue(customerStateManager.BuyingPrice);
    }
}