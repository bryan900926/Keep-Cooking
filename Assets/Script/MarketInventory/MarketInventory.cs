using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketInventory : MonoBehaviour
{
    public List<MarketSlot> slots = new();
    public TMP_InputField[] inputs = new TMP_InputField[4];
    public int maxSlots = 9;
    public int page = 1;

    public static MarketInventory Instance;

    private float Revenue => ScoreManager.Instance.Revenue;

    readonly private Dictionary<int, MarketSlot> slotDictionary = new();

    public event Action OnInventoryUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            int index = i;

            inputs[i].onValueChanged.AddListener((value) =>
            {
                if (((page - 1) * 4 + index) < maxSlots)
                {
                    int.TryParse(value, out int num);

                    MarketSlot slotData = slots[(page - 1) * 4 + index];
                    slotData.Currentcount = Math.Min(num, slotData.limited);
                    MarketUI.Instance.RefreshUI(page);
                }
                else
                {
                    Debug.Log("No Update");
                }

            });
        }
        for (int i = 0; i < slots.Count; i++)
        {
            MarketSlot marketSlot = slots[i];
            slotDictionary[marketSlot.item.Mask] = slots[i];
        }
    }

    public void AddItem(IngredientData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item) slot.amount += amount;
        }
    }
    public void DecreaseItem(IngredientData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item) slot.amount -= amount;
        }
    }
    public void ChangeLimit(IngredientData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item) slot.limited = amount;
        }
    }

    public void ChangePrice(IngredientData item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item) slot.price = amount;
        }
    }

    public int AllTotal()
    {
        int total = 0;
        foreach (var slot in slots)
        {
            total += slot.Currentcount * slot.price;
        }
        return total;
    }

    public void Purchase()
    {
        int totalCost = AllTotal();
        bool anyItemPurchased = false;
        foreach (var slot in slots)
        {
            if (slot.Currentcount > 0)
            {
                // slot.limited -= slot.Currentcount;
                AddItem(slot.item, slot.Currentcount);
                MarketUI.Instance.RefreshUI(page);
                anyItemPurchased = true;
                slot.Currentcount = 0;
            }
        }
        if (anyItemPurchased)
        {
            OnInventoryUpdated?.Invoke();
            ScoreManager.Instance.AddRevenue(-totalCost);
        }

    }

    public void UpdateMenu()
    {
        float multiplier = Mathf.Pow(2, 0.1f);
        foreach (var slot in slots)
        {
            slot.price = (int)(slot.price * multiplier);
        }

        MarketUI.Instance.RefreshUI(page);
    }

    public bool IsIngredientSufficient(Ingredients ingredient, int requiredAmount)
    {
        int itemMask = (int)ingredient;
        if (slotDictionary.TryGetValue(itemMask, out MarketSlot slot))
        {
            return slot.amount >= requiredAmount;
        }
        return false;
    }

    // Tries to consume ingredients for a LIST of dishes at once.
    // Atomic: Either ALL are consumed, or NONE are consumed.
    public bool TryConsumeIngredientsForBatch(List<DishProperty> dishes, float wasteProb)
    {
        // 1. TALLY UP THE TOTAL NEEDS
        // We use a temporary dictionary to sum up how much of each ingredient we need total.
        // Key = Ingredient Mask (int), Value = Total Amount Needed
        Dictionary<int, int> totalRequirements = new();

        for (int i = 0; i < dishes.Count; i++)
        {
            DishProperty dish = dishes[i];

            foreach (var ingredient in dish.normal_recipe)
            {
                if (ingredient == Ingredients.None) continue;

                int mask = (int)ingredient;
                int usingCount = UnityEngine.Random.Range(0f, 1f) < wasteProb ? 2 : 1;
                if (totalRequirements.ContainsKey(mask))
                {
                    totalRequirements[mask] += usingCount;
                }
                else
                {
                    totalRequirements.Add(mask, usingCount);
                }
            }
        }

        // 2. CHECK IF WE HAVE ENOUGH (The "Look" Phase)
        foreach (var req in totalRequirements)
        {
            int mask = req.Key;
            int amountNeeded = req.Value;

            if (!slotDictionary.TryGetValue(mask, out MarketSlot slot) || slot.amount < amountNeeded)
            {
                return false;
            }
        }

        // 3. CONSUME (The "Leap" Phase)
        // If we got here, we guarantee we have enough for EVERYTHING.
        bool stockChanged = false;
        foreach (var req in totalRequirements)
        {
            int mask = req.Key;
            int amountToTake = req.Value;
            slotDictionary[mask].amount -= amountToTake;
            stockChanged = true;
        }
        // 4. NOTIFY
        if (stockChanged)
        {
            MarketUI.Instance.RefreshUI(page);
            OnInventoryUpdated?.Invoke();
        }

        return true;
    }
}
