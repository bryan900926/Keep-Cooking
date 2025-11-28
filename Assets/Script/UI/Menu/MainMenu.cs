using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

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
    }
    void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }
}
