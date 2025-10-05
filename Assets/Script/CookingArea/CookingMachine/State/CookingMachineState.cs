public abstract class CookingMachineState
{
    protected CookingMachineStateManager cookingMachineStateManager;
    public CookingMachineState(CookingMachineStateManager cookingMachineStateManager)
    {
        this.cookingMachineStateManager = cookingMachineStateManager;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
