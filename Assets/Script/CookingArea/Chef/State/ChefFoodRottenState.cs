using UnityEngine;

public class ChefFoodRottenState : ChefState
{
    private GameObject rottenFood;

    public ChefFoodRottenState(ChefStateManager chefStateManager, GameObject rottenFood) : base(chefStateManager)
    {
        this.rottenFood = rottenFood;
    }

    public override void Update()
    {
        if (rottenFood == null)
        {
            chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
            return;
        }
    }

}
