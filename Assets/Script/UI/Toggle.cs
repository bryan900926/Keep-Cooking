using UnityEngine;
using UnityEngine.InputSystem;

public class Toggle : MonoBehaviour
{
    [SerializeField] private GameObject controlPanel;
    private bool isOpen = false;

    void Start()
    {
        controlPanel.SetActive(isOpen);
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            ToggleControlPanel();
        }
    }

    void ToggleControlPanel()
    {
        isOpen = !isOpen;
        controlPanel.SetActive(isOpen);
    }
}
