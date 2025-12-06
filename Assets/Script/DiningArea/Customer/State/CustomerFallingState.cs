public class CustomerFallingState : CustomerState
{
    public CustomerFallingState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Enter()
    {
        customerStateManager.CustomerSFX.FallInHole();
    }


    public override void Update()
    {
        // No update needed for falling state
    }

    public override void Exit()
    {
        // No exit actions needed for falling state
    }
}