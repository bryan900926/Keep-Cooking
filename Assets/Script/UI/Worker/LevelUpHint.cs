using UnityEngine;
using TMPro;

public class LevelUpHint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelUpText;
    public float moveUpSpeed = 50f;
    public float fadeDuration = 1.5f;

    private CanvasGroup canvasGroup;
    private Vector3 startPosition;

    void Awake()
    {
        canvasGroup = levelUpText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = levelUpText.gameObject.AddComponent<CanvasGroup>();

        startPosition = levelUpText.rectTransform.localPosition;
    }

    public void ShowLevelUpHint()
    {
        levelUpText.text = "LEVEL UP!";
        StopAllCoroutines();
        StartCoroutine(AnimateHint());
    }

    private System.Collections.IEnumerator AnimateHint()
    {
        canvasGroup.alpha = 1f;
        levelUpText.rectTransform.localPosition = startPosition;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // Move text upward
            levelUpText.rectTransform.localPosition = startPosition + Vector3.up * moveUpSpeed * (elapsed / fadeDuration);
            // Fade out
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
