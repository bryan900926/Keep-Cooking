using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;

    void Start()
    {
        // Start with the panel closed
        inventoryPanel.SetActive(isOpen);
    }

    void Update()
    {
        Debug.Log(Keyboard.current);
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            Debug.Log("I pressed!");
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }
}
