using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChefCookingState : ChefState
{
    float cookingTime;
    readonly List<OrderInfo> orderInfos;
    List<DishProperty> dishProperties;

    // Flag to control flow
    private bool isWaitingForIngredients = false;

    readonly private Energy energy;

    float wastedIngredientProb = 0f;

    public ChefCookingState(ChefStateManager chefStateManager, List<OrderInfo> orderInfos) : base(chefStateManager)
    {
        this.cookingTime = Random.Range(3f, 5f);
        this.orderInfos = orderInfos;
        energy = chefStateManager.GetComponent<Energy>();
        wastedIngredientProb = 1f - energy.EnergyRatio; // Between 10% to 50%
    }

    public override void Enter()
    {
        dishProperties = Recipe.instance.dishProperties
            .Where(dish => orderInfos.Any(order => order.FoodIdx == dish.Foodidx))
            .ToList();
        isWaitingForIngredients = !MarketInventory.Instance.TryConsumeIngredientsForBatch(dishProperties, wastedIngredientProb);
        if (isWaitingForIngredients)
        {
            EnterWaitingState();
        }
        else
        {
            StartCookingProcess();
        }
    }

    public override void Update()
    {
        // 2. If waiting for ingredients, do NOT decrement the timer
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
        chefStateManager.CurrentDishIdxs.Clear();
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();

        // 3. SAFETY: Always unsubscribe when leaving the state to prevent memory leaks
        if (MarketInventory.Instance != null)
        {
            MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished;
        }
    }

    private List<int> GetTotalIngredientCounts()
    {
        List<int> totalCounts = new List<int>();
        foreach (var dish in dishProperties)
        {
        }
        return totalCounts;
    }
    private void StartCookingProcess()
    {
        // Only play the cooking animation if we are actually cooking
        chefStateManager.HandleSideEffectFlicker(false, Color.yellow);
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().ChangeToCookState();

        // If we were listening for updates, stop listening now that we have what we need
        MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished;
    }

    private void EnterWaitingState()
    {
        // Optional: Set machine to Idle or show a "Missing Ingredients" icon above Chef
        chefStateManager.CookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();

        // 4. Subscribe to the event. This puts the ball in MarketInventory's court.
        // We only subscribe if we aren't already to avoid duplicate calls.
        MarketInventory.Instance.OnInventoryUpdated -= OnInventoryReplenished; // Safety remove first
        MarketInventory.Instance.OnInventoryUpdated += OnInventoryReplenished;
        chefStateManager.HandleSideEffectFlicker(true, Color.yellow);
    }

    // 5. This function is ONLY called when MarketInventory says something changed
    private void OnInventoryReplenished()
    {
        bool success = MarketInventory.Instance.TryConsumeIngredientsForBatch(dishProperties, wastedIngredientProb);

        if (success)
        {
            isWaitingForIngredients = false;
            StartCookingProcess();
        }
        else
        {
            isWaitingForIngredients = true;
        }
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