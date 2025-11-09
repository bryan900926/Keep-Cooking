public class DishState
{
    protected DishStateManager dishStateManager;

    public DishState(DishStateManager dishStateManager)
    {
        this.dishStateManager = dishStateManager;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}