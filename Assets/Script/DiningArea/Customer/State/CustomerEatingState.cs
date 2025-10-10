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
        elapsedTime = 0f;
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= eatingDuration)
        {
            // Finished eating, transition to leaving state
            customerStateManager.DiningSystem.FreeSeat(customerStateManager.DiningIdx);
            customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
        }
    }

    public override void Exit()
    {
        customerStateManager.DiningSystem.FreeSeat(customerStateManager.DiningIdx);
        customerStateManager.DiningIdx = -1;
        GameObject food = customerStateManager.GetComponent<Holding>().HoldingItem;
        if (food != null)
        {
            Object.Destroy(food);
        }
    }
}