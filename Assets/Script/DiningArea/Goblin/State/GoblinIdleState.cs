using Unity.VisualScripting;
using UnityEngine;

public class GoblinIdleState : GoblinState
{
    readonly private Transform[] idlePositions;

    private float roamingTime = 2f;

    public GoblinIdleState(GoblinStateManager stateManager, Transform[] idlePositions) : base(stateManager)
    {
        this.idlePositions = idlePositions;
    }

    public override void Enter()
    {
        roamingTime = Random.Range(3f, 10f);
        stateManager.DestinationSetter.target = idlePositions[Random.Range(0, idlePositions.Length)];
    }

    public override void Update()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0)
        {
            stateManager.ChangeState(new GoblinRobbingState(stateManager));
            return;
        }
        if (Vector3.Distance(stateManager.transform.position, stateManager.DestinationSetter.target.position) < 0.1f)
        {
            stateManager.DestinationSetter.target = idlePositions[Random.Range(0, idlePositions.Length)];
        }
    }

    public override void Exit()
    {
        stateManager.DestinationSetter.target = null;
    }

}