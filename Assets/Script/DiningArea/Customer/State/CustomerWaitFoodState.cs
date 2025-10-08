using UnityEngine;

public class CustomerWaitFoodState : CustomerState
{
    public CustomerWaitFoodState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Enter()
    {
        // Move to dining table seat
        if (customerStateManager.DiningIdx != -1)
        {
            customerStateManager.destinationSetter.target = customerStateManager.diningSystem.seats[customerStateManager.DiningIdx].transform;
        }
        else
        {
            Debug.LogError("No dining seat assigned! This should not happen!");
        }
    }

    public override void Update()
    {
        if (customerStateManager.OrderedFoodIdx == -1 &&
    Vector2.Distance(customerStateManager.diningSystem.seats[customerStateManager.DiningIdx].transform.position,
                     customerStateManager.transform.position) <= 0.5f)
        {
            customerStateManager.OrderedFoodIdx = Menu.Instance.RandomSpawnForCustomer(customerStateManager.gameObject);

        }
    }

    public override void Exit()
    {
    }
}