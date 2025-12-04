using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReputationSystem : MonoBehaviour
{
    public static ReputationSystem Instance { get; private set; }
    float reputationLevel = 0f;
    readonly float maxReputation = 100f;
    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider slider;


    public static Action OnReputationChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (reputationText != null) reputationText.gameObject.SetActive(false);
    }
    public void IncreaseReputation(float amount)
    {
        reputationLevel += amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, maxReputation);
        OnReputationChanged?.Invoke();
        UISFX.Instance.Addreputation();

        if (reputationText != null)
            ShowPopupText(amount, Color.green, "+");
    }
    public void DecreaseReputation(float amount)
    {
        reputationLevel -= amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, maxReputation);
        OnReputationChanged?.Invoke();
        UISFX.Instance.Minusreputation();

        if (reputationText != null)
            ShowPopupText(amount, Color.red, "-");


        if (fillImage != null)
            FlashFillImage();
    }

    public float GetReputationRatio()
    {
        return reputationLevel / maxReputation;
    }

    public float GetReputationLevel()
    {
        return reputationLevel;
    }

    private void ShowPopupText(float amount , Color color , String sign)
    {
        reputationText.gameObject.SetActive(true);
        reputationText.text = sign + $"{amount}";
        reputationText.color = color;
        reputationText.transform.localScale = Vector3.zero;

        Vector3 startPos = reputationText.transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(reputationText.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
        seq.Append(reputationText.transform.DOScale(1f, 0.1f));
        seq.Join(reputationText.transform.DOMoveY(startPos.y + 1f, 1f).SetEase(Ease.OutCubic));
        seq.Join(reputationText.DOFade(0, 1f).SetDelay(0.5f));
        seq.OnComplete(() =>
        {
            reputationText.gameObject.SetActive(false);
            reputationText.transform.position = startPos;
            reputationText.transform.localScale = Vector3.one;
            reputationText.color = Color.red;
        });
    }

    private void FlashFillImage()
    {
        if (fillImage == null) return;

        Color originalColor = Color.white; 
        fillImage.DOKill();

        fillImage.DOColor(Color.red, 0.1f)
                 .SetLoops(2, LoopType.Yoyo)
                 .OnComplete(() => fillImage.color = originalColor);

        slider.transform.DOShakePosition(0.2f, 5f, 20, 90);
    }
}
