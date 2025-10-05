
public class CookingMachineOnFireState : CookingMachineState
{
    public CookingMachineOnFireState(CookingMachineStateManager cookingMachineStateManager) : base(cookingMachineStateManager) { }
    public override void Enter()
    {
        cookingMachineStateManager.fires.SetActive(true);
    }

    public override void Exit()
    {
        cookingMachineStateManager.fires.SetActive(false);
    }
}