using UnityEngine;

public class ChefCookingState : ChefState
{
    public ChefCookingState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        // Cooking already initialized in EnableCooking
    }

    public override void Update()
    {
        chefStateManager.CookingTime -= Time.deltaTime;
        if (chefStateManager.CookingTime <= 0f)
        {
            chefStateManager.CreateDish();
            chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
        }
    }
}
