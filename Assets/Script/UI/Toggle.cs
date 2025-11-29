using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Toggle : MonoBehaviour
{
    public static Toggle Instance;
    [Header("UI Configuration")]
    [SerializeField] private CanvasGroup[] uiElements;
    [SerializeField] private KeysForUI[] keyElements;

    private readonly Dictionary<KeysForUI, CanvasGroup> keyToPanel = new();
    private readonly Dictionary<KeysForUI, bool> toggleStates = new();

    private readonly List<KeysForUI> keysList = new();

    private CanvasGroup uiRootCanvasGroup;

    private bool gameEnd = false;

    private readonly Dictionary<Key, KeysForUI> keyMapping = new()
    {
        { Key.I, KeysForUI.Inventory },
        { Key.V, KeysForUI.Menu },
        { Key.Escape, KeysForUI.Settings },
    };

    public CanvasGroup UIRootCanvasGroup
    {
        get { return uiRootCanvasGroup; }
        set { uiRootCanvasGroup = value; }
    }
    private static readonly string mainSceneName = "Chaos Kitchen";

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

    void Update()
    {
        if (Keyboard.current == null || gameEnd) return;

        if (SceneManager.GetActiveScene().name != mainSceneName) return;

        foreach (var kvp in keyToPanel)
        {
            KeysForUI key = kvp.Key;
            // Safe lookup
            var pair = keyMapping.FirstOrDefault(x => x.Value == key);
            if (pair.Equals(default(KeyValuePair<Key, KeysForUI>)))
                continue; // Skip keys that are not mapped to keyboard

            if (Keyboard.current[pair.Key].wasPressedThisFrame)
            {
                TogglePanel(key);
                break;
            }
        }
    }

    public void TogglePanel(KeysForUI key)
    {

        if (!keyToPanel.ContainsKey(key)) return;

        bool newState = !toggleStates[key];
        if (newState)
        {
            OpenPanel(key);

            if (key == KeysForUI.Settings)
            {
                GameManager.Instance.PauseGame();
                UImanager.Instance.Settingchangemenu();
                toggleStates[key] = !toggleStates[key];
            }
        }
        else ClosePanel(key);
    }

    //public void RegisterPanel(KeysForUI key, CanvasGroup panel)
    //{
    //    if (!keyToPanel.ContainsKey(key))
    //    {
    //        uiElements.Append(panel);
    //        keyElements.Append(key);
    //        keyToPanel[key] = panel;
    //        toggleStates[key] = false;
    //        SetPanelVisibility(panel, false);
    //        keysList.Add(key);
    //    }
    //}

    public void RegisterPanel(KeysForUI key, CanvasGroup panel)
    {
        if (keyToPanel.ContainsKey(key))
        {
            keyToPanel[key] = panel;
            toggleStates[key] = false; 
            Debug.Log($"[Toggle] Updated existing panel for key: {key}");
        }
        else
        {
            keyToPanel.Add(key, panel);
            toggleStates.Add(key, false);
            keysList.Add(key);
            Debug.Log($"[Toggle] Registered new panel for key: {key}");
        }

        SetPanelVisibility(panel, false);
        keyElements = keysList.ToArray();
        uiElements = keysList.Select(k => keyToPanel[k]).ToArray();
    }

    public void UnregisterPanel(KeysForUI key)
    {
        if (keyToPanel.ContainsKey(key))
        {
            CanvasGroup panel = keyToPanel[key];
            uiElements = uiElements.Where(e => e != panel).ToArray();
            keyElements = keyElements.Where(k => k != key).ToArray();
            keyToPanel.Remove(key);
            toggleStates.Remove(key);
            keysList.Remove(key);
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

    public void OpenPanel(KeysForUI key)
    {
        if (!keyToPanel.ContainsKey(key)) return;
        ToggleUIRoot(true);
        toggleStates[key] = true;
        SetPanelVisibility(keyToPanel[key], true);
    }

    public void ToggleUIRoot(bool enable)
    {
        if (uiRootCanvasGroup != null)
        {
            uiRootCanvasGroup.blocksRaycasts = enable;
        }
    }

    public void ClosePanel(KeysForUI key)
    {
        if (!keyToPanel.ContainsKey(key)) return;
        toggleStates[key] = false;
        if (uiRootCanvasGroup != null && !keysList.Any(k => toggleStates[k]))
        {
            ToggleUIRoot(false);
        }
        SetPanelVisibility(keyToPanel[key], false);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainSceneName)
        {
            // Automatically find your UI root
            uiRootCanvasGroup = GameObject.FindWithTag("UIRoot")?.GetComponent<CanvasGroup>();

            if (uiRootCanvasGroup == null)
                Debug.LogWarning("UIRoot CanvasGroup not found in Chaos Kitchen scene!");
            else
            {
                Debug.Log("UIRoot CanvasGroup assigned successfully!");
                uiRootCanvasGroup.blocksRaycasts = false;
            }
        }

        foreach (var key in keysList)
        {
            toggleStates[key] = false;
            Debug.Log($"toggle state for {toggleStates[key]}");
            Debug.Log(keyToPanel.ContainsKey(key));
        }
    }

    public void SetGameEnd(bool isGameEnd)
    {
        gameEnd = isGameEnd;
    }
}
