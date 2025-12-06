using DG.Tweening;
using UnityEngine;

public class Timeripple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PlayTimeRipple(Vector3 center)
    {   

        SpriteRenderer rippleSR = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.position = center;

        // 藍色光澤
        rippleSR.color = new Color(0.4f, 0.7f, 1f, 0.6f);


        // 橢圓形擴散（水平大、垂直小）
        gameObject.transform.DOScale(new Vector3(12f, 6f, 1f), 1.5f)
            .SetEase(Ease.OutQuad);


        // 淡出
        rippleSR.DOFade(0f, 1.8f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(gameObject));
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
