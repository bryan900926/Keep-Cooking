using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private TextMeshProUGUI revenueText;
    [SerializeField] private TextMeshProUGUI serverdCntText;

    [SerializeField] private float fadeDuration = 2.0f;

    private bool isShown = false;

    void Start()
    {
        retryBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });

        exitBtn.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
            UImanager.Instance.ClickShowUI(UImanager.MenuOptions.main);
        });
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (TimeManager.Instance.RemainingTime <= 0 && !isShown)
        {
            Show();
        }
    }

    public void Show()
    {
        isShown = true;

        UISFX.Instance.PlayGameOver();

        revenueText.SetText($"Revenue: ${ScoreManager.Instance.Revenue:F2}");
        serverdCntText.SetText($"Customers Served: {ScoreManager.Instance.ServedCnt}");
        StartCoroutine(FadeInRoutine());
        GameManager.Instance.PauseGame();
        Toggle.Instance.UIRootCanvasGroup.blocksRaycasts = true;
        Toggle.Instance.SetGameEnd(true);
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            // Use unscaledDeltaTime so it works even if the game is paused
            timer += Time.unscaledDeltaTime;

            // Calculate progress (0 to 1)
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            // Apply to CanvasGroup
            if (canvasGroup != null) canvasGroup.alpha = alpha;

            yield return null; // Wait for next frame
        }

        // Ensure it ends at exactly 1 and enable interaction
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
