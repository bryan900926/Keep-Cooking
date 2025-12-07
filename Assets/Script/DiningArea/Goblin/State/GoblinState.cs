public abstract class GoblinState
{
    protected GoblinStateManager stateManager;

    public GoblinState(GoblinStateManager stateManager)
    {
        this.stateManager = stateManager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}