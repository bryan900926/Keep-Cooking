using UnityEngine;
using UnityEngine.InputSystem;

public class Toggle : MonoBehaviour
{
    [SerializeField] private CanvasGroup controlPanel;
    private bool isOpen = false;

    void Start()
    {
        UpdatePanelVisibility();
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;
            UpdatePanelVisibility();
        }
    }

    void UpdatePanelVisibility()
    {
        controlPanel.alpha = isOpen ? 1f : 0f;
        controlPanel.interactable = isOpen;
        controlPanel.blocksRaycasts = isOpen;
    }
}
