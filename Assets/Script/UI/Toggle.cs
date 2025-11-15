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
    [SerializeField] private Key[] keyElements;

    private readonly Dictionary<Key, CanvasGroup> keyToPanel = new();
    private readonly Dictionary<Key, bool> toggleStates = new();

    private List<Key> keysList = new();

    public static readonly Key keyOpenCrafting = Key.V;

        private CanvasGroup uiRootCanvasGroup;

    public CanvasGroup UIRootCanvasGroup
    {
        get { return uiRootCanvasGroup; }
        set { uiRootCanvasGroup = value; }
    }

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

    public void UnregisterPanel(Key key)
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

    public void OpenPanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;
        if (uiRootCanvasGroup != null)
        {
            uiRootCanvasGroup.blocksRaycasts = true;
        }
        toggleStates[key] = true;
        SetPanelVisibility(keyToPanel[key], true);
    }

    public void ClosePanel(Key key)
    {
        if (!keyToPanel.ContainsKey(key)) return;
        toggleStates[key] = false;
        if (uiRootCanvasGroup != null && !keysList.Any(k => toggleStates[k]))
        {
            uiRootCanvasGroup.blocksRaycasts = false;
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
        if (scene.name == "Chaos Kitchen")
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
    }
}
