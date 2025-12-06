using UnityEngine;
using DG.Tweening;
using System.Collections;
using Pathfinding;

public class Freeze : MonoBehaviour
{
    public float minFreezeTime = 1.5f;
    public float maxFreezeTime = 2f;
    [SerializeField] private GameObject ghostPrefab;
    private bool CanFreeze = true;

    public void TriggerFreeze(GameObject customer)
    {
        if (!CanFreeze) return;
        
        CanFreeze = false;
        
        float freezeDuration = Random.Range(minFreezeTime, maxFreezeTime);

        Vector3 originalPos = customer.transform.position;



        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(
        ghostPrefab,
        customer.transform.position,
        customer.transform.rotation
        );

            ghost.GetComponent<CustomerStateManager>().enabled = false;
            ghost.GetComponent<Freeze>().CanFreeze = false;
            Rigidbody2D rb = ghost.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = false;   // 完全不參與物理
            }

            SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1, 1, 1, 0.3f); // 淡淡的半透明
            }

            Animator ghostAnim = ghost.GetComponent<Animator>();
            Animator customerAnim = customer.GetComponent<Animator>();

            // 2. 同步動作（並靜止）
            if (ghostAnim != null && customerAnim != null)
            {
                AnimatorStateInfo state = customerAnim.GetCurrentAnimatorStateInfo(0);
                ghostAnim.Play(state.shortNameHash, 0, state.normalizedTime);
                ghostAnim.Update(0f);
                ghostAnim.speed = 0f; // 靜止在同一個姿勢
            }

            // 3. DOTween 動畫：推出 → 回來 → 淡出
            Vector3 startPos = customer.transform.position;
            Vector3 pushPos = startPos + new Vector3(1f, 0f, 0f); // 往右推開 1f

            Sequence seq = DOTween.Sequence();

            seq.Append(ghost.transform.DOMove(pushPos, 0.5f).SetEase(Ease.OutQuad))   
               .Append(ghost.transform.DOMove(startPos, 0.5f).SetEase(Ease.InQuad)) 
               .Join(sr.DOFade(0f, 0.5f))                                           
               .OnComplete(() =>
               {
                   Destroy(ghost);
               });
        }

        StartCoroutine(FreezeCoroutine(customer, freezeDuration));
    }

    private IEnumerator FreezeCoroutine(GameObject customer, float duration)
    {
        Vector3 freezePos = customer.transform.position;
        SpriteRenderer sd = customer.GetComponent<SpriteRenderer>();
        Color originalColor = sd.color;

        sd.DOColor(new Color(0.4f, 0.7f, 1f, 0.6f), 0.1f);

        float elapsed = 0f;
        var ai = customer.GetComponent<AIPath>();
        ai.isStopped = true;

        while (elapsed < duration)
        {
            customer.transform.position = freezePos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        sd.DOColor(originalColor, 0.2f);

        ai.isStopped = false;
    }
}

