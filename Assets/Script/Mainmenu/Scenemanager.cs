using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private CanvasGroup cg;
    public static SceneController Instance;
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

    private void Start()
    {

    }
    public void ChangeScene(string sceneName)
    {
        UISFX.Instance.PlayButtonClick();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        UISFX.Instance.PlayButtonClick();
        Debug.Log("Exit");
        Application.Quit();
    }
}
