public abstract class BeerMachineState
{
    protected BeerMachineStateManager stateManager;

    public BeerMachineState(BeerMachineStateManager manager)
    {
        stateManager = manager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}