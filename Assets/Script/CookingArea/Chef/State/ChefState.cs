// Base state
public abstract class ChefState
{
    protected ChefStateManager chefStateManager;

    public ChefState(ChefStateManager chefStateManager)
    {
        this.chefStateManager = chefStateManager;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}