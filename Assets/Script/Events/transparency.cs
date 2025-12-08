using DG.Tweening;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float fadeDuration = 1f;

    private SpriteRenderer sr;
    private Collider2D col;


    void Awake()
    {

    }

    public void StartInvisible(GameObject gameObject)
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        Sequence seq = DOTween.Sequence()
            .Append(sr.DOFade(0f, fadeDuration));    // 消失
    }
}


