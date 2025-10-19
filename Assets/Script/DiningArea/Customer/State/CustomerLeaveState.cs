using UnityEngine;

public class CustomerLeaveState : CustomerState
{
    private const string EXIT_TAG = "Exit";

    private Transform exitPoint;
    public CustomerLeaveState(CustomerStateManager customerStateManager) : base(customerStateManager)
    {
    }

    public override void Enter()
    {
        exitPoint = GameObject.FindGameObjectWithTag(EXIT_TAG).transform;
        customerStateManager.DestinationSetter.target = exitPoint;
    }

    public override void Update()
    {
        if (IsAtExit() && customerStateManager.DiningIdx != -1)
        {
            DiningSystem.Instance.FreeSeat(customerStateManager.DiningIdx);
            customerStateManager.DiningIdx = -1;
            Object.Destroy(customerStateManager.gameObject);
        }

    }

    bool IsAtExit()
    {
        return Vector3.Distance(customerStateManager.transform.position, exitPoint.position) < 0.1f;
    }

    private void RandomMenuEffect()
    {
        Recipe.instance.BuildOtherRecipeDictionary();
        CenterMessage.Instance.ShowMessage(CenterMessage.MENU_UPDATED);
    }
}