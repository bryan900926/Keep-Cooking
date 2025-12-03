using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customrep : MonoBehaviour
{
    [Header("UI 元件")]
    public CanvasGroup canvasGroup;
    public Image emojiImage;
    public TextMeshProUGUI text;

    [Header("動畫參數")]
    public float popupScale = 1.3f;
    public float popupDuration = 0.25f;
    public float fadeDuration = 0.4f;

    private Vector3 baseScale;
    

    void Awake()
    {
        baseScale = transform.localScale;
        canvasGroup.alpha = 0;
    }

    public void ShowFeedback(Sprite emoji, string msg, Color color)
    {
        emojiImage.sprite = emoji;
        text.text = msg;
        text.color = color;

        canvasGroup.alpha = 1;
        transform.localScale = baseScale * 0.6f;

        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1, 0.1f));
        seq.Join(transform.DOScale(popupScale, popupDuration).SetEase(Ease.OutBack));

        seq.AppendInterval(0.4f);

        seq.Append(canvasGroup.DOFade(0, fadeDuration));
        seq.Join(transform.DOMoveY(transform.position.y + 0.4f, fadeDuration));

        seq.OnComplete(() =>
        {
            canvasGroup.alpha = 0;
            transform.localScale = baseScale;
        });
    }
}
