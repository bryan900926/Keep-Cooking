using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private bool isOpen = false;

    void Start()
    {
        // Start with the panel closed
        inventoryPanel.SetActive(isOpen);
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }
}
