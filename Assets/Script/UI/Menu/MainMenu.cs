using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private Button tutorialBtn;

    void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("Chaos Kitchen");
            UImanager.Instance.ClickHideUI();
            GameManager.Instance.StartClick();
        });
        optionBtn.onClick.AddListener(() =>
        {
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.option);
        });
        exitBtn.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.main);
        });
        tutorialBtn.onClick.AddListener(() =>
        {
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.tutorial);
        });
    }
    void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }
}
