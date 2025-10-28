using UnityEngine;

public class CustomerToChefState : CustomerState
{
    private int cookerIdx = -1;
    private Transform exitPoint;
    private const string EXIT_TAG = "Exit";

    public CustomerToChefState(CustomerStateManager customerStateManager, int cookerIdx) : base(customerStateManager)
    {
        this.cookerIdx = cookerIdx;
    }

    public override void Enter()
    {
        exitPoint = GameObject.FindGameObjectWithTag(EXIT_TAG).transform;
        GameObject holdItem = customerStateManager.GetComponent<Holding>().RemoveHolding();
        if (holdItem)
        {
            Object.Destroy(holdItem);
        }
        Debug.Log($"{customerStateManager.gameObject.name} is going to chef at cooker index {cookerIdx}.");
        DiningSystem.Instance.FreeSeat(customerStateManager.DiningIdx);
        customerStateManager.DestinationSetter.target = exitPoint;
    }

    public override void Update()
    {
        if (Vector2.Distance(customerStateManager.transform.position, customerStateManager.DestinationSetter.target.position) < 0.1f)
        {
            Debug.Log($"{customerStateManager.gameObject.name} reached exit, becoming chef at cooker index {cookerIdx}.");
            BackControl.Instance.AssignTask(cookerIdx, customerStateManager.WorkerData.id);
            Object.Destroy(customerStateManager.gameObject);
        }
    }

    public override void Exit()
    {
    }
}