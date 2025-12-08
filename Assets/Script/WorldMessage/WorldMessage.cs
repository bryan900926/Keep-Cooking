using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WorldMessage : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private RectTransform root;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform bgImage;
    [SerializeField] private RectTransform textRoot;

    public float fadeTime = 0.3f;
    public float stayTime = 2f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void Show(string msg, Vector3 worldPos, MessageFlip flip)
    {
        text.text = msg;
        transform.position = worldPos;

        ApplyFlip(flip);

        cg.alpha = 0;
        cg.DOFade(1, fadeTime)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(stayTime, () =>
                {
                    cg.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject));
                });
            });
    }

    private void ApplyFlip(MessageFlip flip)
    {
        switch (flip)
        {
            case MessageFlip.Left:
                bgImage.localScale = new Vector3(-1, 1, 1); // Â½Âà­I´º
                textRoot.localScale = new Vector3(-1, 1, 1);  // ©è®øÂ½Âà
                break;

            case MessageFlip.Right:
                bgImage.localScale = Vector3.one;
                textRoot.localScale = Vector3.one;
                break;

            case MessageFlip.Up:
                bgImage.localScale = new Vector3(1, -1, 1);
                textRoot.localScale = new Vector3(1, -1, 1);
                break;

            case MessageFlip.Down:
                bgImage.localScale = Vector3.one;
                textRoot.localScale = Vector3.one;
                break;
        }
    }
}
public enum MessageFlip
{
        Left,
        Right,
        Up,
        Down
}
