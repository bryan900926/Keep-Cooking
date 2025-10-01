using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;


public class NavBarController : MonoBehaviour
{
    [SerializeField] GameObject frontPanel;
    [SerializeField] GameObject backPanel;

    [SerializeField] Button frontBtn;
    [SerializeField] Button backBtn;

    void Start()
    {
        ShowPanel(frontPanel); // default
        frontBtn.onClick.AddListener(() => ShowPanel(frontPanel));
        backBtn.onClick.AddListener(() => ShowPanel(backPanel));
    }

    public void ShowPanel(GameObject panel)
    {
        frontPanel.SetActive(false);
        backPanel.SetActive(false);

        panel.SetActive(true);
    }
}
