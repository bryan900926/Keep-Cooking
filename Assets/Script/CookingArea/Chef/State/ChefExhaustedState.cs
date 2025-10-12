using System.Collections.Generic;
using UnityEngine;

public class ChefExhaustedState : ChefState
{
    private int cookIdx;
    public ChefExhaustedState(ChefStateManager chefStateManager, int cookIdx) : base(chefStateManager)
    {
        this.cookIdx = cookIdx;
    }

    public override void Enter()
    {
        RemoveWorkerData();
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
        chefStateManager.Destination = chefStateManager.LeaveTarget.transform;
        chefStateManager.DestinationSetter.target = chefStateManager.Destination;

    }

    public override void Update()
    {
        if (chefStateManager.Destination == null) return;
        float dist = Vector3.Distance(chefStateManager.transform.position, chefStateManager.Destination.position);
        if (dist < 0.1f)
        {
            GameObject.Destroy(chefStateManager.gameObject);
        }
    }

    private void RemoveWorkerData()
    {
        Dictionary<int, int> mapper = BackControl.Instance.Mapper;
        if (mapper.TryGetValue(cookIdx, out int controllUiIdx))
        {
            mapper.Remove(cookIdx);
            BackWorkerUIManager.Instance.RemoveWorkerInfoUI(controllUiIdx);
        }
        else
        {
            Debug.LogWarning("worker cannot quit the job");
        }
    }

}
