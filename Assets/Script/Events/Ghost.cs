using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Ghost : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image blackOverlay;
    [SerializeField] private Transform transforms;
    public void Awake()
    {
        blackOverlay = GameObject.Find("BlackOverlay").GetComponent<Image>();
    }

    public void Update()
    {
        blackOverlay.DOFade(0.35f, 0.3f);
    }
    public void Appear()
    {
        if (blackOverlay == null)
        {
            Debug.LogError("BlackOverlay not assigned!");
            return;
        }

        var sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 999;
        sr.color = new Color(1, 1, 1, 0);

        transform.position = new Vector3(15f, -5f, 0f);
        blackOverlay.color = new Color(0, 0, 0, 0);

        DOTween.Sequence()
            .Append(blackOverlay.DOFade(0.5f, 0.3f))
            .Join(sr.DOFade(0.6f, 0.5f))
            .Join(transform.DOMoveX(-15f, 1f).SetEase(Ease.InQuad))
            .Append(sr.DOFade(0f, 0.5f))
            .Append(blackOverlay.DOFade(0f, 0.3f))
            .OnComplete(() =>
            {
                blackOverlay.DOFade(0f, 0.3f);
                Destroy(gameObject);   // ¦A Destroy Ghost
            });
    }

}
