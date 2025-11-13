using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Toggle : MonoBehaviour
{
    public static Toggle Instance;
    [Header("UI Configuration")]
    [SerializeField] private CanvasGroup[] uiElements;
    [SerializeField] private Key[] keyElements;

    private readonly Dictionary<Key, CanvasGroup> keyToPanel = new();
    private readonly Dictionary<Key, bool> toggleStates = new();

    private List<Key> keysList = new();

    public static readonly Key keyOpenCrafting = Key.V;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (SceneManager.GetActiveScene().name != "Chaos Kitchen") return;

        foreach (var kvp in keyToPanel)
        {
            Key key = kvp.Key;

            if (Keyboard.current[key].wasPressedThisFrame)
            {
                TogglePanel(key);
                break;
            }
        }
    }

    private void TogglePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        bool newState = !toggleStates[key];
        if (newState){
            OpenPanel(key);

            if (key == Key.Escape)
            {
                GameManager.Instance.PauseGame();
                toggleStates[key] = !toggleStates[key];
            }
        }
        else ClosePanel(key);
    }

    public void RegisterPanel(Key key, CanvasGroup panel)
    {
        if (!keyToPanel.ContainsKey(key))
        {
            uiElements.Append(panel);
            keyElements.Append(key);
            keyToPanel[key] = panel;
            toggleStates[key] = false;
            SetPanelVisibility(panel, false);
            keysList.Add(key);
            Debug.Log(key);
        }
    }
    private static void SetPanelVisibility(CanvasGroup panel, bool visible)
    {
        if (panel == null) return;

        panel.alpha = visible ? 1f : 0f;
        panel.interactable = visible;
        panel.blocksRaycasts = visible;
    }

    public void CloseAllUIPanels()
    {
        foreach (var key in keysList)
            toggleStates[key] = false;

        foreach (var panel in keyToPanel.Values.ToList())
            SetPanelVisibility(panel, false);
    }

    public void OpenPanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        toggleStates[key] = true;
        SetPanelVisibility(keyToPanel[key], true);
    }

    public void ClosePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;

        toggleStates[key] = false;
        SetPanelVisibility(keyToPanel[key], false);
    }
}
