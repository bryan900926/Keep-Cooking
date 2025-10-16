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
            if (chefStateManager.CurrentDishIdx != -2)
            {
                chefStateManager.CreateDish();
                chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
            }
            else
            {
                GameObject leftover = chefStateManager.CreateLeftover();
                chefStateManager.ChangeState(new ChefFoodRottenState(chefStateManager, leftover));
            }

        }
    }

    public override void Exit()
    {
        chefStateManager.CurrentDishIdx = -1;
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
    }
}
