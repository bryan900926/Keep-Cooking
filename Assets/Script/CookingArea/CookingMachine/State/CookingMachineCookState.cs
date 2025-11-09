public class CookingMachineCookState : CookingMachineState
{
    public CookingMachineCookState(CookingMachineStateManager cookingMachineStateManager) : base(cookingMachineStateManager)
    {
    }

    public override void Enter()
    {
        cookingMachineStateManager.SmokeEffect.SetActive(true);
    }


    public override void Exit()
    {
        cookingMachineStateManager.SmokeEffect.SetActive(false);
    }
}