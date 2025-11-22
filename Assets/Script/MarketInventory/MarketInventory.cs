using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class MarketInventory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<MarketSlot> slots = new List<MarketSlot>();
    public TMP_InputField[] inputs = new TMP_InputField[4];
    public int maxSlots = 9;
    public int page = 1;

    public static MarketInventory Instance;

    private void Awake()
    {
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
                    int num = 0;
                    int.TryParse(value, out num);

                    MarketSlot slotData = slots[(page - 1) * 4 + index];
                    slotData.Currentcount = num;

                    MarketUI.Instance.RefreshUI(page);
                }
                else
                {
                    Debug.Log("No Update");
                }   
                    
            });
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
        foreach (var slot in slots)
        {
            if (slot.Currentcount > 0 &&¡@slot.limited >= slot.Currentcount)
            {   
                slot.limited -= slot.Currentcount;
                AddItem(slot.item, slot.Currentcount);
                slot.Currentcount = 0;
                MarketUI.Instance.RefreshUI(page);
                // Need Economy System to deduct money
            }
        }
    }

    public void UpdateMenu()
    {
        foreach (var slot in slots)
        {
            if (slot.Currentcount > 0 && slot.limited >= slot.Currentcount)
            {
                slot.limited -= slot.Currentcount;
                AddItem(slot.item, slot.Currentcount);
                slot.Currentcount = 0;
                MarketUI.Instance.RefreshUI(page);
                // Need Economy System to deduct money
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (page == 3)
        {

        }
    }
}
