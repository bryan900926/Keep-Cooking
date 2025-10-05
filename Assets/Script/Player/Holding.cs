using UnityEngine;

public class Holding : MonoBehaviour
{
    private GameObject holdingItem;
    public GameObject HoldingItem => holdingItem;

    public void SetHoldingItem(GameObject newItem)
    {
        if (!newItem)
        {
            Debug.LogError("item cannot be null");
        }
        holdingItem = newItem;
    }

    public void RemoveHolding()
    {
        holdingItem = null;
    }
}
