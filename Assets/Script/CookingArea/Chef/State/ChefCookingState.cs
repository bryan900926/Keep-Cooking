using System.Collections.Generic;
using UnityEngine;
public class ChefCookingState : ChefState
{
    float cookingTime;
    readonly List<OrderInfo> orderInfos;

    public ChefCookingState(ChefStateManager chefStateManager, List<OrderInfo> orderInfos) : base(chefStateManager)
    {
        this.cookingTime = UnityEngine.Random.Range(3f, 5f);
        this.orderInfos = orderInfos;
    }

    public override void Enter()
    {
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().ChangeToCookState();
    }

    public override void Update()
    {
        cookingTime -= Time.deltaTime;
        if (cookingTime <= 0f)
        {
            CreateDish();
            chefStateManager.ChangeState(new ChefDeliverFoodState(chefStateManager));

        }
    }

    public override void Exit()
    {
        chefStateManager.CurrentDishIdxs.Clear();
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
    }

    public void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;

        foreach (int dishIdx in orderInfos.ConvertAll(order => order.FoodIdx))
        {
            if (dishIdx != -1 && dishIdx < menu.Length && chefStateManager.CookIdx != -1)
            {
                GameObject food = Menu.Instance.SpawnForPlayer(dishIdx, (Vector2)chefStateManager.transform.position);
                PickUpV2 pickUp = food.GetComponent<PickUpV2>();
                pickUp.Pick(chefStateManager.gameObject);
                pickUp.Pickable = false;
            }
        }
    }
}
