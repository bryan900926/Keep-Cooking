using UnityEngine;

public class CustomerToChefState : CustomerState
{
    private int cookerIdx = -1;
    public CustomerToChefState(CustomerStateManager customerStateManager, int cookerIdx) : base(customerStateManager)
    {
        this.cookerIdx = cookerIdx;
    }

    public override void Enter()
    {
        GameObject holdItem = customerStateManager.GetComponent<Holding>().RemoveHolding();
        if (holdItem)
        {
            Object.Destroy(holdItem);
        }
        Debug.Log($"{customerStateManager.gameObject.name} is going to chef at cooker index {cookerIdx}.");
        DiningSystem.Instance.FreeSeat(customerStateManager.DiningIdx);
        Transform cookerSpot = BackControl.Instance.GetCookers[cookerIdx].GetComponent<CookingSpot>().GetSpot;
        customerStateManager.DestinationSetter.target = cookerSpot;
        int uiIdx = BackWorkerUIManager.Instance.FillWorkerInfoUI(customerStateManager.gameObject);
        BackControl.Instance.Mapper.Add(cookerIdx, uiIdx);
        if (uiIdx != -1)
        {
            BackControl.Instance.Mapper[cookerIdx] = uiIdx;
        }

    }

    public override void Update()
    {
        if (Vector2.Distance(customerStateManager.transform.position, customerStateManager.DestinationSetter.target.position) < 0.1f)
        {
            customerStateManager.GetComponent<ChefStateManager>().enabled = true;
            customerStateManager.GetComponent<ChefStateManager>().Initialize(cookerIdx);
            customerStateManager.GetComponent<Energy>().Reset();
            customerStateManager.GetComponent<CustomerStateManager>().enabled = false;
        }
    }

    public override void Exit()
    {
    }
}