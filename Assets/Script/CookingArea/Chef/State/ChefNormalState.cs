using UnityEngine;
public class ChefNormalState : ChefState
{
    private bool isCooking = false;
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
        if (!isCooking)
        {
            TableOrder();
        }
    }

    private void TableOrder()
    {
        OrderInfo order = OrderSystem.Instance.GetOrderForChef();
        if (order == null) return;
        if (chefStateManager.CheckChefForgetRecipe())
        {
            Debug.Log("@Chef forgot recipe, randomizing");
            chefStateManager.GetComponent<ChefRecipe>().RandomizeRecipe(order.FoodIdx);
        }
        isCooking = true;
        if (Recipe.instance.CheckRecipeCorrect(order.FoodIdx, chefStateManager.GetComponent<ChefRecipe>().GetRecipe(order.FoodIdx)))
        {
            chefStateManager.EnableCooking(order.FoodIdx);
        }
        else
        {
            Debug.Log("@Chef forgot recipe, randomizing");
            chefStateManager.EnableCooking(-2); // -2 for leftover
            OrderSystem.Instance.AddFailOrder(order, false);
        }
    }
}
