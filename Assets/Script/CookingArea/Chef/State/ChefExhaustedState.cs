using System.Collections;
using UnityEngine;

public class ChefExhaustedState : ChefState
{
    public ChefExhaustedState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        if (chefStateManager.Cooker != null)
        {
            chefStateManager.Cooker.SetDestination(chefStateManager.LeaveTarget.transform);
            chefStateManager.StartCoroutine(QuitWhenArrived());
        }
    }

    private IEnumerator QuitWhenArrived()
    {
        while (Vector2.Distance(chefStateManager.transform.position, chefStateManager.LeaveTarget.transform.position) > 0.1f)
        {
            yield return null;
        }
        chefStateManager.Cooker.HandleQuitJob();
    }
}
