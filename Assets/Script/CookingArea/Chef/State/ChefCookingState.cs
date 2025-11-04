using System.Collections.Generic;
using UnityEngine;
public class ChefCookingState : ChefState
{
    float cookingTime;
    List<OrderInfo> orderInfos;
    public ChefCookingState(ChefStateManager chefStateManager, float cookingTime, List<OrderInfo> orderInfos) : base(chefStateManager)
    {
        this.cookingTime = cookingTime;
        this.orderInfos = orderInfos;
    }

    public override void Enter()
    {
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

                float wrongProb = Mathf.Clamp01(1 - chefStateManager.Energy.CurrentEnergy / chefStateManager.Energy.MaxEnergy);

                if (UnityEngine.Random.value < wrongProb)
                {
                    chefStateManager.SetFireActive(true);
                }
                else
                {
                    chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
                }

                Vector2 spawnPos = (Vector2)chefStateManager.transform.position + Vector2.right;
                GameObject food = Menu.Instance.SpawnForPlayer(dishIdx, spawnPos);
                PickUpV2 pickUp = food.GetComponent<PickUpV2>();
                pickUp.Pick(chefStateManager.gameObject);
                pickUp.Pickable = false;
            }
        }
    }
}
