using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Toggle : MonoBehaviour
{
    public static Toggle Instance;
    [Header("UI Configuration")]
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private Key[] keyElements;

    private readonly Dictionary<Key, CanvasGroup> keyToPanel = new();
    private readonly Dictionary<Key, bool> toggleStates = new();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        InitializePanels();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        foreach (var key in keyToPanel.Keys.ToList())
        {
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                TogglePanel(key);
                break;
            }
        }
    }

    // 🧩 Initialize mappings
    private void InitializePanels()
    {
        if (uiElements.Length != keyElements.Length)
        {
            Debug.LogError("UI elements and key elements arrays must be of the same length.");
            return;
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            var ui = uiElements[i];
            var key = keyElements[i];

            if (!ValidateUIElement(ui, key)) continue;

            var canvas = ui.GetComponent<CanvasGroup>();
            keyToPanel[key] = canvas;
            toggleStates[key] = false;

            SetPanelVisibility(canvas, false);
        }
    }

    // ✅ Toggle logic — close others, open selected
    private void TogglePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        bool newState = !toggleStates[key];
        CloseAllUIPanels();  // Ensure only one is open

        toggleStates[key] = newState;
        SetPanelVisibility(keyToPanel[key], newState);
    }

    // ✅ Helper to set visibility
    private static void SetPanelVisibility(CanvasGroup panel, bool visible)
    {
        if (panel == null) return;

        panel.alpha = visible ? 1f : 0f;
        panel.interactable = visible;
        panel.blocksRaycasts = visible;
    }

    // ✅ Closes all managed UI panels
    public void CloseAllUIPanels()
    {
        foreach (var key in toggleStates.Keys.ToList())
            toggleStates[key] = false;

        foreach (var panel in keyToPanel.Values.ToList())
            SetPanelVisibility(panel, false);
    }

    // ✅ Open any specific panel (e.g. from button)
    public void OpenPanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        CloseAllUIPanels();
        toggleStates[key] = true;
        SetPanelVisibility(keyToPanel[key], true);
    }

    // ✅ For buttons (like Craft)
    public void OpenCraftUI()
    {
        // If you know the craft key (for example, index 1 or Key.C):
        var key = keyElements.Length > 1 ? keyElements[1] : Key.C;
        OpenPanel(key);
    }

    // ✅ Validation helper
    private bool ValidateUIElement(GameObject ui, Key key)
    {
        if (ui == null)
        {
            Debug.LogWarning($"UI element for key {key} is null. Skipping.");
            return false;
        }

        if (ui.GetComponent<CanvasGroup>() == null)
        {
            Debug.LogWarning($"UI element '{ui.name}' has no CanvasGroup. Adding one automatically.");
            ui.AddComponent<CanvasGroup>();
        }

        return true;
    }
}
