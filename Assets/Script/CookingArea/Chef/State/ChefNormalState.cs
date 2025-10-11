using UnityEngine;

public class ChefNormalState : ChefState
{
    public ChefNormalState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        chefStateManager.CurrentDishIdx = -1;
        // already has a destination → don’t recalc
        if (chefStateManager.Destination != null) return;

        // if assigned to a cooking spot
        if (chefStateManager.CookIdx >= 0)
        {
            GameObject[] cookers = BackControl.Instance.GetCookers;
            if (chefStateManager.CookIdx < cookers.Length)
            {
                chefStateManager.Destination = cookers[chefStateManager.CookIdx].GetComponent<CookingSpot>().GetSpot;
                chefStateManager.DestinationSetter.target = chefStateManager.Destination;
            }
            else
            {
                Debug.LogWarning($"cookIdx {chefStateManager.CookIdx} out of range!");
            }
        }
    }

    public override void Update()
    {
    }
}
