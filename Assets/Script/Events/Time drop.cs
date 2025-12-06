using DG.Tweening;
using UnityEngine;

public class Timedrop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject ripplePrefab;
    public Vector3 startposition ;
    public Vector3 hitposition ;
    void Start()
    {
        
    }

    public void PlayWaterDrop(Vector3 start, Vector3 hitPoint)
    {
    
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color = new Color(1, 1, 1, 0); 


        sr.DOFade(1f, 0.4f);


        gameObject.transform.DOMove(hitPoint, 1f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                // 產生 Ripple！
                GameObject ripple = Instantiate(ripplePrefab, transform.position, Quaternion.identity);
                
                ripple.GetComponent<Timeripple>().PlayTimeRipple(hitPoint);

                // 水滴淡出後消失
                sr.DOFade(0f, 0.2f)
                  .OnComplete(() => Destroy(gameObject));
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
