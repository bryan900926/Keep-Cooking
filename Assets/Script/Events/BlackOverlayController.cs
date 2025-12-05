using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackOverlayController : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;


    public void FadeIn(float targetAlpha = 0.35f, float duration = 0.3f)
    {
        if (blackOverlay == null) return;
        blackOverlay.DOKill();                     
        blackOverlay.color = new Color(0, 0, 0, 0); 
        blackOverlay.DOFade(targetAlpha, duration)
            .SetId("BlackFade")
            .OnStart(() => Debug.Log("Black fade in started"))
            .OnComplete(() => Debug.Log("Black fade in complete"));
    }

    public void FadeOut(float duration = 0.4f)
    {
        if (blackOverlay == null) return;
        blackOverlay.DOKill();                    
        blackOverlay.DOFade(0f, duration)
            .SetId("BlackFade")
            .OnStart(() => Debug.Log("Black fade out started"))
            .OnComplete(() => Debug.Log("Black fade out complete"));
    }
}
