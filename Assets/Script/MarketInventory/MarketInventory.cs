using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class MarketInventory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<MarketSlot> slots = new List<MarketSlot>();
    public Dictionary<string, MarketSlot> slotDict = new Dictionary<string, MarketSlot>();
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

        slotDict.Clear();
        foreach (var slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                slotDict[slot.item.Name] = slot;
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
                AddItem(slot.name, slot.Currentcount);
                slot.Currentcount = 0;
                MarketUI.Instance.RefreshUI(page);
                // Need Economy System to deduct money
            }
        }
    }

    public void UpdateMenu() // Inflation every 45 seconds
    {
        float multiplier = Mathf.Pow(2, 0.15f);
        foreach (var slot in slots)
        {
            slot.price = (int) (slot.price * multiplier);
        }
    }

    public void MarketEvent()
    {

    }


    // Update is called once per frame
    void Update()
    {
    }
}
