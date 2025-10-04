public class ChefNormalState : ChefState
{
    public ChefNormalState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        // Could reset animation or UI
        chefStateManager.CurrentDishIdx = -1;
    }

    public override void Update()
    {
    }
}
