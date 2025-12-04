using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customrep : MonoBehaviour
{
    [Header("UI ����")]
    public CanvasGroup canvasGroup;
    public Image emojiImage;
    public TextMeshProUGUI text;

    [Header("�ʵe�Ѽ�")]
    public float popupScale = 1.3f;
    public float popupDuration = 0.25f;
    public float fadeDuration = 0.4f;

    private Vector3 baseScale;

    private Sequence seq;


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

        seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1, 0.1f));
        seq.Join(transform.DOScale(popupScale, popupDuration).SetEase(Ease.OutBack));

        seq.AppendInterval(0.4f);

        seq.Append(canvasGroup.DOFade(0, fadeDuration));

        seq.OnComplete(() =>
        {
            canvasGroup.alpha = 0;
            transform.localScale = baseScale;
        });
    }

    void OnDestroy()
    {
        seq.Kill();
    }
}
