public class CraftBeingUsedState : CraftState
{
    public CraftBeingUsedState(CraftStateManager craftingStateManager) : base(craftingStateManager) { }

    public override void Enter()
    {
        craftStateManager.IsUsing = true;
    }

    public override void Update()
    {
        if (craftStateManager.CurrentChef == null)
        {
            CenterMessage.Instance.ShowMessage(CenterMessage.CHEF_LEAVE);
            craftStateManager.ChangeState(new CraftIdleState(craftStateManager));
        }
    }

    public override void Exit()
    {
        craftStateManager.IsUsing = false;
        Toggle.Instance.ClosePanel(Toggle.keyOpenCrafting);
    }

}