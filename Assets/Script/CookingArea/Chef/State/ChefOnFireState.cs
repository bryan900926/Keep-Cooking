public class ChefOnFireState : ChefState
{

    public ChefOnFireState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        chefStateManager.SetFireActive(true);
    }

    private void ExtinguishFireAfterDelay()
    {
        chefStateManager.SetFireActive(false);
        chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
    }
}
