using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Dizzy : MonoBehaviour
{
    [SerializeField] private Image dizzy;
    private void Start()
    {

    }
    public void StartDizzy()
    {
        RectTransform dizzyRect = dizzy.GetComponent<RectTransform>();

        // 重置角度
        dizzyRect.localRotation = Quaternion.identity;

        // 無限旋轉
        dizzyRect.DOLocalRotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetId(this);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

}
