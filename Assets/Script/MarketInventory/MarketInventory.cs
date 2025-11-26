using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketInventory : MonoBehaviour
{
    public List<MarketSlot> slots = new();
    public Dictionary<string, MarketSlot> slotDict = new();
    public TMP_InputField[] inputs = new TMP_InputField[4];
    public int maxSlots = 9;
    public int page = 1;

    public static MarketInventory Instance;

    public event Action OnInventoryUpdated;

    readonly private Dictionary<string, int> initPrice = new();

    readonly private Dictionary<int, string> Mask2String = new();

    private float startTime;

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
            inputs[i].contentType = TMP_InputField.ContentType.IntegerNumber;
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
            startTime = Time.time;
        }

        slotDict.Clear();
        foreach (var slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                slotDict[slot.item.Name] = slot;
                initPrice[slot.item.Name] = slot.price;
                Mask2String[slot.item.Mask] = slot.item.Name;
            }
        }
    }

    public void AddItem(string name, int amount)
    {
        if (slotDict.ContainsKey(name))
        {
            slotDict[name].amount += amount;
        }
    }
    public void DecreaseItem(string name, int amount)
    {
        if (slotDict.ContainsKey(name))
        {
            slotDict[name].amount -= amount;
        }
    }

    public void ChangeLimit(string name, int amount)
    {
        if (slotDict.ContainsKey(name))
        {
            slotDict[name].limited = amount;
        }
    }

    public void RecoverLimit(int amount)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                slot.limited = amount;
            }
        }
    }

    public void ChangePrice(string name, float amount, bool reverse)
    {
        if (slotDict.ContainsKey(name))
        {
            if (!reverse) slotDict[name].price = (int)(slotDict[name].price * amount);
            else slotDict[name].price = (int)(slotDict[name].price / amount);
        }
    }

    public int AllTotal()
    {
        int total = 0;
        foreach (var slot in slots)
        {
            total += Mathf.Min(slot.Currentcount, slot.limited) * slot.price;
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
                slot.Currentcount = Mathf.Min(slot.Currentcount, slot.limited);
                slot.limited -= slot.Currentcount;
                AddItem(slot.item.Name, slot.Currentcount);
                slot.Currentcount = 0;
                MarketUI.Instance.RefreshUI(page);
                anyItemPurchased = true;
            }
        }
        if (anyItemPurchased)
        {
            ScoreManager.Instance.AddRevenue(-totalCost);
            OnInventoryUpdated?.Invoke();
        }

    }

    public void UpdateMenu() // Inflation every 45 seconds
    {
        float multiplier = Mathf.Pow(2, 0.15f);
        float elapsedTime = Time.time - startTime;
        int intervals = (int)(elapsedTime / 45f);
        foreach (var slot in slots)
        {
            slot.price = (int)(initPrice[slot.item.Name] * Mathf.Pow(multiplier, intervals));
        }
        MarketUI.Instance.RefreshUI(page);
    }


    // Tries to consume ingredients for a LIST of dishes at once.
    // Atomic: Either ALL are consumed, or NONE are consumed.
    public bool TryConsumeIngredientsForBatch(List<DishProperty> dishes, float wasteProb, int id = -1)
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
                if (totalRequirements.ContainsKey(mask))
                {
                    totalRequirements[mask] += 1;
                }
                else
                {
                    totalRequirements.Add(mask, 1);
                }
            }
        }

        // 2. CHECK IF WE HAVE ENOUGH (The "Look" Phase)
        foreach (var req in totalRequirements)
        {
            int mask = req.Key;
            int amountNeeded = req.Value;
            string name = Mask2String[mask];

            if (!slotDict.TryGetValue(name, out MarketSlot slot) || slot.amount < amountNeeded)
            {
                return false;
            }
            else if (!slotDict.ContainsKey(name))
            {
                Debug.LogError("Ingredient not found in inventory: " + name);
            }
        }

        // 3. CONSUME (The "Leap" Phase)
        // If we got here, we guarantee we have enough for EVERYTHING.
        foreach (var req in totalRequirements)
        {
            int mask = req.Key;
            int amountToTake = req.Value;
            if (Mask2String.ContainsKey(mask) == false)
            {
                Debug.LogError("Ingredient mask not found in Mask2String: " + mask);
                continue;
            }
            else
            {
            }
            string name = Mask2String[mask];
            Debug.Log("@Consuming Ingredient: " + mask + " Name: " + Mask2String[mask]);
            slotDict[name].amount -= amountToTake;
        }
        Debug.Log("@@ " + id + " Consumed ingredients for batch.");
        // 4. NOTIFY
        MarketUI.Instance.RefreshUI(page);

        return true;
    }
}
