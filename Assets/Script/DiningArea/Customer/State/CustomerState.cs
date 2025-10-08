public class CustomerState
{
    protected CustomerStateManager customerStateManager;

    public CustomerState(CustomerStateManager customerStateManager)
    {
        this.customerStateManager = customerStateManager;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}