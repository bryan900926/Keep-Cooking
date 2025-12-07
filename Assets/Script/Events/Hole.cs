using DG.Tweening;
using UnityEngine;

public class Hole : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Holemanager manager;
    private CircleCollider2D col;
    void Start()
    {
        manager = FindFirstObjectByType<Holemanager>();
        col = GetComponent<CircleCollider2D>();
    }

    public void HoleAction()
    {
        DOTween.Sequence()
        .Append(gameObject.transform.DOScale(Vector3.one, 0f))
        .AppendInterval(1f)
        .AppendCallback(() => col.enabled = false)  
        .Append(gameObject.transform.DOScale(Vector3.zero, 1f))
        .OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<CustomerSFX>(out var customerSFX))
        {
            if (other.TryGetComponent<CustomerStateManager>(out var customerStateManager))
            {
                if (customerStateManager.CurrentState is CustomerFallingState)
                {
                    return;
                }
                customerStateManager.ChangeState(new CustomerFallingState(customerStateManager));
                manager.TriggerHole(other.gameObject);
            }
        }
    }
}
