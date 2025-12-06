using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    [SerializeField] private Button backBtn;

    private Tutorial tutorial;

    void Start()
    {
        tutorial = GetComponent<Tutorial>();
        backBtn.onClick.AddListener(() =>
        {
            UISFX.Instance.PlayButtonClick();
            tutorial.ClearAllObJects();
            UImanager.Instance.ReturnUI();
        });
    }

    private void OnDestroy()
    {
        backBtn.onClick.RemoveAllListeners();
    }
}