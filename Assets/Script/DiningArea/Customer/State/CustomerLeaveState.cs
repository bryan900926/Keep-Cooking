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
        if (customerStateManager.DiningIdx != -1)
        {
            Debug.Log("@Customer is leaving from dining seat." + customerStateManager.DiningIdx);
            DiningSystem.Instance.FreeSeat(customerStateManager.DiningIdx);
            customerStateManager.DiningIdx = -1;
        }
        if (customerStateManager.LiningIdx != -1)
        {
            customerStateManager.Qs.FreeSeat(customerStateManager.LiningIdx);
            customerStateManager.LiningIdx = -1;
        }
    }

    public override void Update()
    {
        if (IsAtExit())
        {
            DoorController.Instance.TriggerDoorOpen();
            Object.Destroy(customerStateManager.gameObject);
        }

    }

    bool IsAtExit()
    {
        return Vector3.Distance(customerStateManager.transform.position, exitPoint.position) < 1f;
    }

    private void RandomMenuEffect()
    {
        Recipe.instance.BuildOtherRecipeDictionary();
        CenterMessage.Instance.ShowMessage(CenterMessage.MENU_UPDATED);
    }
}