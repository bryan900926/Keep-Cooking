public abstract class CraftState
{
    protected CraftStateManager craftStateManager;

    public CraftState(CraftStateManager craftStateManager)
    {
        this.craftStateManager = craftStateManager;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }
}