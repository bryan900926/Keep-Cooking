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

    private List<Key> keysList;

    public static readonly Key keyOpenCrafting = Key.V;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        keysList = keyElements.ToList();
        InitializePanels();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        foreach (var key in keysList)
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

    private void TogglePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        bool newState = !toggleStates[key];
        if (newState)
            OpenPanel(key);
        else
            ClosePanel(key);
    }
    private static void SetPanelVisibility(CanvasGroup panel, bool visible)
    {
        if (panel == null) return;

        panel.alpha = visible ? 1f : 0f;
        panel.interactable = visible;
        panel.blocksRaycasts = visible;
    }

     void CloseAllUIPanels()
    {
        foreach (var key in keysList)
            toggleStates[key] = false;

        foreach (var panel in keyToPanel.Values.ToList())
            SetPanelVisibility(panel, false);
    }

    public void OpenPanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        CloseAllUIPanels();
        toggleStates[key] = true;
        SetPanelVisibility(keyToPanel[key], true);
    }

    public void ClosePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        toggleStates[key] = false;
        SetPanelVisibility(keyToPanel[key], false);
    }

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
