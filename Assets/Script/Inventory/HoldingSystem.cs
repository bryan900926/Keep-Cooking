using UnityEngine;

/// <summary>
/// Manages the holding slots for ingredients.
/// </summary>
public class HoldingSystem : MonoBehaviour
{
    public GameObject[] slots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = GameObject.FindGameObjectsWithTag("HoldingSlot");

    }

    public void AddIngredient(IngredientData ingredient)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            HoldingSlot slot = slots[i].GetComponent<HoldingSlot>();
            if (slot.currentItem == null)
            {
                Debug.Log("Slot " + i + " is empty");
                slot.currentItem = ingredient;
                break;
            }
        }
    }
}
