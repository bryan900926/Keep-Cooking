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
            customerStateManager.DestinationSetter.target = DiningSystem.Instance.seats[customerStateManager.DiningIdx].transform;
        }
        else
        {
            Debug.LogError("No dining seat assigned! This should not happen!");
        }
    }

    public override void Exit()
    {
    }
}