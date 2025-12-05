using System.Collections.Generic;
using UnityEngine;

public class Holding : MonoBehaviour
{
    private readonly List<GameObject> holdingItem = new();

    public static Vector3 baseOffset = new(0, 1.5f, 0); // Base height above the player
    public float horizontalSpacing = 1f; // Distance between items
    private int maxHoldingItem = 1;      // You can increase as needed

    public List<GameObject> HoldingItem => holdingItem;
    public int HoldingCount => holdingItem.Count;

    public int AvailableSpace => maxHoldingItem - holdingItem.Count;

    public void RemoveAllHolding()
    {
        foreach (var item in holdingItem)
        {
            Object.Destroy(item);
        }
        holdingItem.Clear();
    }

    public void AddCapacity()
    {
        maxHoldingItem++;
    }

    public bool HasSpace()
    {
        return holdingItem.Count < maxHoldingItem;
    }

    public void PickUpItem(GameObject item)
    {
        if (!item || holdingItem.Count >= maxHoldingItem)
            return;

        holdingItem.Add(item);
        item.transform.SetParent(transform);

        UpdateItemPositions();
    }

    public void RemoveHoldingItem(GameObject item)
    {
        if (holdingItem.Remove(item))
        {
            item.transform.SetParent(null);
            UpdateItemPositions();
        }
    }

    public void RemoveLeftoverItems()
    {
        foreach (var item in holdingItem)
        {
            if (item.GetComponent<PickUpV2>().FoodIdx == -2)
            {
                Object.Destroy(item);
                holdingItem.Remove(item);
            }
        }
        UpdateItemPositions();
    }

    private void UpdateItemPositions()
    {
        if (holdingItem.Count == 0)
            return;

        // Center items horizontally relative to the player
        float totalWidth = (holdingItem.Count - 1) * horizontalSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < holdingItem.Count; i++)
        {
            Vector3 offset = baseOffset + new Vector3(startX + i * horizontalSpacing, 0, 0);
            holdingItem[i].transform.localPosition = offset;
        }
    }

    public bool FindBeer()
    {
        foreach (var item in holdingItem)
        {
            if (item.TryGetComponent(out PickUpV2 pickUpV2))
            {
                if (pickUpV2.FoodIdx == 3)
                {
                    RemoveHoldingItem(item);
                    Destroy(item);
                    return true;
                }
            }
        }
        return false;
    }
}
