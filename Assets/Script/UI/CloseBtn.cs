using UnityEngine;
using UnityEngine.UI;

public class CloseBtn : MonoBehaviour
{
    Button closeButton;
    [SerializeField] private KeysForUI key;
    void Start()
    {
        closeButton = GetComponent<Button>();
        closeButton.onClick.AddListener(ClosePanel);
    }
    void ClosePanel()
    {
        UISFX.Instance.PlayButtonClick();
        Toggle.Instance.ClosePanel(key);
    }
}
