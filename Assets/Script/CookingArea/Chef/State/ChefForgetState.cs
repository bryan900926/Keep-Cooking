using System.Collections.Generic;
using UnityEngine;

public class ChefForgetState : ChefState
{
    private readonly List<OrderInfo> orderInfos;

    public ChefForgetState(ChefStateManager chefStateManager, List<OrderInfo> orderInfos) : base(chefStateManager)
    {
        this.orderInfos = orderInfos;
    }

    public override void Enter()
    {
        Debug.Log("Chef has forgotten the recipe!");
        foreach (var order in orderInfos)
        {
            chefStateManager.GetComponent<ChefRecipe>().RandomizeRecipe(order.FoodIdx);
        }
        chefStateManager.SweatEffect.SetActive(true);
    }

    public override void Update()
    {
        if (chefStateManager.ChefHasCorrectRecipe)
        {
            chefStateManager.ChangeState(new ChefCookingState(chefStateManager, orderInfos));
        }
    }

    public override void Exit()
    {
        chefStateManager.SweatEffect.SetActive(false);
    }
}