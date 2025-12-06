using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Timeripple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float rippleDuration = 2f;
    public Vector3 targetScale = new Vector3(12f, 6f, 1f); 
    private BoxCollider2D col;
    private HashSet<CustomerStateManager> affected = new HashSet<CustomerStateManager>();

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider2D>();

        col.isTrigger = true; 
    }
    void Start()
    {
        
    }


    public void PlayTimeRipple(Vector3 center)
    {   

        SpriteRenderer rippleSR = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.position = center;
        rippleSR.color = new Color(0.4f, 0.7f, 1f, 0.6f);

        Vector2 initialSize = col.size;
        Vector2 targetSize = new Vector2(12f, 6f);

        gameObject.transform.DOScale(targetScale, rippleDuration).SetEase(Ease.OutQuad);

        DOTween.To(() => col.size, x => col.size = x, targetSize, rippleDuration).SetEase(Ease.OutQuad);

        rippleSR.DOFade(0f, rippleDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(gameObject));
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var customer = other.GetComponent<CustomerStateManager>();
        if (customer == null) return;
        if (affected.Contains(customer)) return;

        var freeze = other.GetComponent<Freeze>();
        if (freeze == null) return;

        affected.Add(customer);
        freeze.TriggerFreeze(customer.gameObject);
    }
}
