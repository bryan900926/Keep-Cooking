using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public enum MenuOptions
    {
        option,
        main,
        setting,

        tutorial
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UImanager Instance;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject tutorial;
    private Dictionary<MenuOptions, CanvasGroup> canvasgroup = new();
    private MenuOptions currentMenu;
    private MenuOptions previousMenu;

    private Canvas canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        canvas = GetComponent<Canvas>();
    }
    void Start()
    {
        canvasgroup.Add(MenuOptions.option, options.GetComponent<CanvasGroup>());
        canvasgroup.Add(MenuOptions.main, main.GetComponent<CanvasGroup>());
        canvasgroup.Add(MenuOptions.setting, setting.GetComponent<CanvasGroup>());
        canvasgroup.Add(MenuOptions.tutorial, tutorial.GetComponent<CanvasGroup>());
        currentMenu = MenuOptions.main;
        previousMenu = MenuOptions.main;


        HideAllUI();
        ShowUI(canvasgroup[currentMenu]);
    }
    public void ReturnUI()
    {
        ClickShowUI(previousMenu);
    }

    public void Settingchangemenu()
    {
        currentMenu = MenuOptions.setting;
    }

    public void HideUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ClickHideUI()
    {
        UISFX.Instance.PlayButtonClick();
        HideUI(canvasgroup[currentMenu]);
    }

    public void ClickShowUI(MenuOptions name)
    {
        previousMenu = currentMenu;
        UISFX.Instance.PlayButtonClick();
        currentMenu = name;
        HideAllUI();
        ShowUI(canvasgroup[currentMenu]);
    }

    private void HideAllUI()
    {
        foreach (var kvp in canvasgroup)
        {
            HideUI(kvp.Value);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas.worldCamera = Camera.main;
    }

}
