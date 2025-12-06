using System;
using System.Collections.Generic;
using System.Linq;
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
            slotDict[name].amount = Mathf.Clamp(slotDict[name].amount - amount, 0, slotDict[name].amount);
        }
    }

    public void Disappear()
    {
        var keys = slotDict.Keys.ToList();
        int count = Mathf.Min(3, keys.Count);

        var rnd = new System.Random();
        var selected = new HashSet<int>();

        while (selected.Count < count)
        {
            int index = rnd.Next(keys.Count);
            selected.Add(index); 
        }

        foreach (int i in selected)
        {
            slotDict[keys[i]].amount = 0;
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

                var mask = slot.item.Mask;
                if (slot.amount > 0)
                {
                    LowStockReminder.Instance.RemoveLowStockIngredient(mask);
                }
                else
                {
                    LowStockReminder.Instance.AddLowStockIngredient(mask);
                }
            }
        }
        if (anyItemPurchased)
        {
            ScoreManager.Instance.AddRevenue(-totalCost);
            UISFX.Instance.PlayPurchaseItem();
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
    public bool TryConsumeIngredientsForBatch(Dictionary<int, int> totalRequirements, int id = -1)
    {
        foreach (var req in totalRequirements.ToList())   // avoid modifying while iterating
        {
            int mask = req.Key;
            int amountNeeded = req.Value;

            if (amountNeeded <= 0) continue;

            string name = Mask2String[mask];

            if (!slotDict.TryGetValue(name, out MarketSlot slot))
            {
                Debug.LogError("Ingredient not found in inventory: " + name);
                continue;
            }

            // Fetch as many as available
            int takenCnt = Mathf.Min(amountNeeded, slot.amount);

            slot.amount -= takenCnt;
            if (slot.amount > 0)
            {
                LowStockReminder.Instance.RemoveLowStockIngredient(mask);
            }
            else if (slot.amount == 0)
            {
                LowStockReminder.Instance.AddLowStockIngredient(mask);
            }
            totalRequirements[mask] -= takenCnt;
        }

        // Return true only when all requirements are satisfied
        return totalRequirements.Values.All(v => v <= 0);

    }
}
