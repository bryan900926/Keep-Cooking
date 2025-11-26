using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChefCookingState : ChefState
{
    float cookingTime;
    readonly List<OrderInfo> orderInfos;
    List<DishProperty> dishProperties;

    private bool isWaitingForIngredients = false;

    private bool hasConsumedIngredients = false;   // 🔒 NEW: prevents multi-consume

    readonly private Energy energy;
    float wastedIngredientProb = 0f;

    public ChefCookingState(ChefStateManager chefStateManager, List<OrderInfo> orderInfos)
        : base(chefStateManager)
    {
        this.cookingTime = Random.Range(3f, 5f);
        this.orderInfos = orderInfos;
        energy = chefStateManager.GetComponent<Energy>();
        wastedIngredientProb = 0f;
    }

    public override void Enter()
    {
        Debug.Log("@ChefCookingState:" + chefStateManager.GetInstanceID() + " Entering Cooking State");

        // Get the required dishes
        dishProperties = Recipe.instance.dishProperties
            .Where(dish => orderInfos.Any(order => order.FoodIdx == dish.Foodidx))
            .ToList();

        // 🔒 Only try consuming ONCE per cycle
        if (!hasConsumedIngredients)
        {
            isWaitingForIngredients =
                !MarketInventory.Instance.TryConsumeIngredientsForBatch(
                    dishProperties,
                    wastedIngredientProb,
                    chefStateManager.gameObject.GetInstanceID()
                );

            if (!isWaitingForIngredients)
            {
                hasConsumedIngredients = true;
            }
        }

        // Decide next step
        if (isWaitingForIngredients)
            EnterWaitingState();
        else
            StartCookingProcess();
    }

    public override void Update()
    {
        // Waiting → do nothing
        if (isWaitingForIngredients) return;

        cookingTime -= Time.deltaTime;

        if (cookingTime <= 0f)
        {
            CreateDish();
            chefStateManager.ChangeState(new ChefDeliverFoodState(chefStateManager));
        }
    }

    public override void Exit()
    {
        // Cleanup
        chefStateManager.CurrentDishIdxs.Clear();
        chefStateManager.CookingMachine
            .GetComponent<CookingMachineStateManager>()
            .SetBackToNormal();

        // Unsubscribe event
        if (MarketInventory.Instance != null)
        {
            MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished;
        }

        // 🔄 Reset for next cooking cycle
        hasConsumedIngredients = false;
        isWaitingForIngredients = false;
    }

    private void StartCookingProcess()
    {
        chefStateManager.CookingMachine
            .GetComponent<CookingMachineStateManager>()
            .ChangeToCookState();

        chefStateManager.HandleLowStockEffect(false);

        // Stop listening
        MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished;
    }

    private void EnterWaitingState()
    {
        Debug.Log("ChefCookingState: EnterWaitingState, waiting for ingredients");

        chefStateManager.CookingMachine
            .GetComponent<CookingMachineStateManager>()
            .SetBackToNormal();

        // Subscribe exactly once
        MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished;
        MarketInventory.Instance.OnInventoryUpdated += OnInventoryReplenished;

        chefStateManager.HandleLowStockEffect(true);
    }

    private void OnInventoryReplenished()
    {
        if (!isWaitingForIngredients || hasConsumedIngredients)
            return;

        Debug.Log("@ChefCookingState → OnInventoryReplenished");

        bool success = MarketInventory.Instance.TryConsumeIngredientsForBatch(
            dishProperties,
            wastedIngredientProb,
            chefStateManager.gameObject.GetInstanceID()
        );

        if (success)
        {
            Debug.Log("@ChefCookingState → Ingredients obtained after wait");

            hasConsumedIngredients = true;
            isWaitingForIngredients = false;
            StartCookingProcess();
        }
    }

    public void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;

        foreach (int dishIdx in orderInfos.ConvertAll(order => order.FoodIdx))
        {
            if (dishIdx != -1 && dishIdx < menu.Length && chefStateManager.CookIdx != -1)
            {
                GameObject food = Menu.Instance.SpawnForPlayer(
                    dishIdx,
                    (Vector2)chefStateManager.transform.position
                );

                PickUpV2 pickUp = food.GetComponent<PickUpV2>();
                pickUp.Pick(chefStateManager.gameObject);
                pickUp.Pickable = false;
            }
        }
    }
}
