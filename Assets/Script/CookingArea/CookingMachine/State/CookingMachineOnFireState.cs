
public class CookingMachineOnFireState : CookingMachineState
{
    public CookingMachineOnFireState(CookingMachineStateManager cookingMachineStateManager) : base(cookingMachineStateManager) { }
    public override void Enter()
    {
        cookingMachineStateManager.Fires.SetActive(true);
    }

    public override void Exit()
    {
        cookingMachineStateManager.Fires.SetActive(false);
    }
}