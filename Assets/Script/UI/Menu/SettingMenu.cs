using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;

    void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            UImanager.Instance.ClickHideUI();
            GameManager.Instance.ResumeGame();
        });
        restartBtn.onClick.AddListener(() =>
        {
            UISFX.Instance.PlayButtonClick();
            GameManager.Instance.RestartGame();
        });
        optionBtn.onClick.AddListener(() =>
        {
            UISFX.Instance.PlayButtonClick();
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.option);
        });
        exitBtn.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.main);
        });
    }
    private void OnDestroy()
    {
        continueBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }
}
