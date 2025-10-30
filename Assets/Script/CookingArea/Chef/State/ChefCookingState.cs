using UnityEngine;
public class ChefCookingState : ChefState
{
    float cookingTime;
    public ChefCookingState(ChefStateManager chefStateManager, float cookingTime) : base(chefStateManager)
    {
        this.cookingTime = cookingTime;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        cookingTime -= Time.deltaTime;
        if (cookingTime <= 0f)
        {
            if (chefStateManager.CurrentDishIdx != -2)
            {
                chefStateManager.CreateDish();
                chefStateManager.ChangeState(new ChefDeliverFoodState(chefStateManager));
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
