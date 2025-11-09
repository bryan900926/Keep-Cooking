public class WaiterState
{
    protected WaiterStateManager waiterStateManager;

    public WaiterState(WaiterStateManager waiterStateManager)
    {
        this.waiterStateManager = waiterStateManager;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}