using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Assign your gameplay root (everything that should pause)")]
    [SerializeField] private GameObject gameplayRoot;

    private bool isPaused = false;
    private readonly List<MonoBehaviour> disabledScripts = new();

    public bool IsPaused => isPaused;

    private const string GAMEPLAY_ROOT_TAG = "GameRoot";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;

        // Freeze time
        Time.timeScale = 0f;

        // Disable all scripts inside GameplayRoot (but not UI)
        disabledScripts.Clear();
        foreach (var mb in gameplayRoot.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (mb.enabled)
            {
                mb.enabled = false;
                disabledScripts.Add(mb);
            }
        }

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;

        // Resume time
        Time.timeScale = 1f;

        // Re-enable scripts that were disabled
        foreach (var mb in disabledScripts)
            if (mb != null)
                mb.enabled = true;

        disabledScripts.Clear();

        Debug.Log("Game Resumed");
    }

    public void RestartGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Toggle.Instance.CloseAllUIPanels();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TimeManager.Instance.ResetTimer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameplayRoot = GameObject.FindWithTag(GAMEPLAY_ROOT_TAG);
        if (gameplayRoot == null) return;
        GameManager.Instance.ResumeGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // Unsubscribe to prevent leaks
    }

    public void StartClick()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
