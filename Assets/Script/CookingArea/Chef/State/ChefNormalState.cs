using UnityEngine;
public class ChefNormalState : ChefState
{
    private bool isCooking = false;
    public ChefNormalState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        chefStateManager.CurrentDishIdxs.Clear();
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
        if (!isCooking)
        {
            chefStateManager.EnableCookingManyFoods();
        }
    }
}
