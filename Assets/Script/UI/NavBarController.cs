using UnityEngine;
using UnityEngine.UI;


public class NavBarController : MonoBehaviour
{
    [SerializeField] GameObject frontPanel;
    [SerializeField] GameObject backPanel;

    [SerializeField] Button frontBtn;
    [SerializeField] Button backBtn;

    void Start()
    {
        ShowPanel(backPanel); // default
        frontBtn.onClick.AddListener(() =>
        {
            UISFX.Instance.PlayButtonClick();
            ShowPanel(frontPanel);
        });
        backBtn.onClick.AddListener(() =>
        {
            UISFX.Instance.PlayButtonClick();
            ShowPanel(backPanel);
        });
    }

    public void ShowPanel(GameObject panel)
    {
        frontPanel.SetActive(false);
        backPanel.SetActive(false);

        panel.SetActive(true);
    }
}
