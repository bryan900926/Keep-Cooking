using System.Collections;
using UnityEngine;

public class ChefOnFireState : ChefState
{
    private float fireDuration = 3f;

    public ChefOnFireState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        chefStateManager.SetFireActive(true);
        chefStateManager.StartCoroutine(ExtinguishFireAfterDelay());
    }

    private IEnumerator ExtinguishFireAfterDelay()
    {
        yield return new WaitForSeconds(fireDuration);
        chefStateManager.SetFireActive(false);
        chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
    }
}
