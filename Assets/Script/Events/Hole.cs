using DG.Tweening;
using UnityEngine;

public class Hole : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Holemanager manager;
    void Start()
    {
        manager = FindFirstObjectByType<Holemanager>();
    }

    public void HoleAction()
    {
        DOTween.Sequence()
        .Append(gameObject.transform.DOScale(Vector3.one, 0f)) 
        .AppendInterval(1f) 
        .Append(gameObject.transform.DOScale(Vector3.zero, 1f)) 
        .OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CustomerStateManager>() != null)
        {
            manager.TriggerHole(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
