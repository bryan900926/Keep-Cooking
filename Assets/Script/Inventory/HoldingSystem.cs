using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the holding slots for ingredients.
/// </summary>
public class HoldingSystem : MonoBehaviour
{

    public static HoldingSystem Instance { get; private set; }
    public GameObject[] slots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        slots = GameObject.FindGameObjectsWithTag("HoldingSlot");

    }

    public void AddProp(PropData prop)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HoldingSlot slot = slots[i].GetComponent<HoldingSlot>();
            if (slot.currentItem == null)
            {
                Debug.Log("Slot " + i + " is empty");
                slot.currentItem = prop;
                break;
            }
        }
    }
    public int FindProp(PropData.Tools propName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HoldingSlot slot = slots[i].GetComponent<HoldingSlot>();
            if (slot.currentItem != null && slot.currentItem.Type == propName)
            {
                return i;
            }
        }
        return -1;
    }
    public void RemoveProp(int idx)
    {
        if (idx < 0 || idx >= slots.Length) return;
        HoldingSlot slot = slots[idx].GetComponent<HoldingSlot>();
        slot.currentItem = null;
    }
}
