using UnityEngine;

public class GoblinLeaveState : GoblinState
{
    public GoblinLeaveState(GoblinStateManager stateManager) : base(stateManager)
    {
    }

    public override void Enter()
    {
        stateManager.DestinationSetter.target = stateManager.ExitPoint;
    }

    public override void Update()
    {
        if (Vector3.Distance(stateManager.transform.position, stateManager.ExitPoint.position) < 0.1f)
        {
            Object.Destroy(stateManager.gameObject);
        }
    }

    public override void Exit()
    {
        stateManager.DestinationSetter.target = null;
    }
}