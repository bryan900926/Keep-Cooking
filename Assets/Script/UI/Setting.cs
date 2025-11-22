using UnityEngine;
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
            pauseButton.onClick.AddListener(PauseGame);
        }
    }
    void PauseGame()
    {
        Debug.Log("Pause Key Pressed");
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
