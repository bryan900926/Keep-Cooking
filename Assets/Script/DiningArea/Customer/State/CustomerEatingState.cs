using UnityEngine;

public class CustomerEatingState : CustomerState
{
    private readonly float eatingDuration; // Duration for eating in seconds
    private float elapsedTime = 0f;
    private readonly float freshness;

    public CustomerEatingState(CustomerStateManager customerStateManager, float freashness) : base(customerStateManager)
    {
        this.freshness = freashness;
        eatingDuration = Random.Range(5f, 10f);
    }

    public override void Enter()
    {
        customerStateManager.CustomerSFX.PlayEating();
        if (customerStateManager.GetComponent<Holding>().HoldingItem.Count == 0)
        {
            Debug.LogError("Customer has no food to eat!");
        }
        if (freshness <= 0)
        {
            customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
            return;
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
        if (freshness > 0)
        {
            GameObject coin = Object.Instantiate(customerStateManager.CoinPrefab);
            coin.GetComponent<Coin>().InitData((int)(customerStateManager.BuyingPrice * 0.5f), customerStateManager.transform);
            ReputationSystem.Instance.IncreaseReputation(5f);
            ScoreManager.Instance.AddRevenue(customerStateManager.BuyingPrice);
        }
        customerStateManager.CustomerSFX.StopEating();
        customerStateManager.CustomerSFX.PaidMoney();
    }
}