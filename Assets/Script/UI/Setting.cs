using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    Button pauseButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton = GetComponent<Button>();
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
            pauseButton.onClick.AddListener(() => UImanager.Instance.ClickShowUI("setting")); ;
        }
    }
    void PauseGame()
    {
        UISFX.Instance.PlayButtonClick();
        Toggle.Instance.TogglePanel(KeysForUI.Settings);
    }

    void OnDestroy()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(PauseGame);
        }
    }
}
