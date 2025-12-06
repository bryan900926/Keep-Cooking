using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;


public class Holemanager : MonoBehaviour
{
    [SerializeField] private GameObject HolePrefab;
    
    public void SandwormEvent(float interval, float duration)
    {
        StartCoroutine(SandwormInvasionCoroutine(interval, duration));
    }
    public IEnumerator SandwormInvasionCoroutine(float interval, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        { 
            int randomindex = Random.Range(1, DiningSystem.Instance.seats.Length);
            HoleGenerator(randomindex);
            
            // 等待 interval 秒
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }
    public void HoleGenerator(int index)
    {
        var dining = DiningSystem.Instance;
        Vector3 pos = dining.seats[index].transform.position;
        Vector3 holePos = pos + new Vector3(0f, -0.4f, 0f); // 往下 0.8 單位
        GameObject Hole = Instantiate(HolePrefab, holePos, Quaternion.identity);
        Hole.GetComponent<Hole>().HoleAction();
    }

    public void TriggerHole(GameObject customer)
    {
        // 動畫或粒子效果（可選）
        PlayHoleAnimation(customer);
    }

    private void PlayHoleAnimation(GameObject customer)
    {
        if (customer == null) return;

        // Tween 動畫
        customer.transform.DOMoveY(customer.transform.position.y - 0.4f, 0.4f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                DiningSystem.Instance.RemoveCustomer(customer);
                DOTween.Kill(customer.transform, false);
                Destroy(customer);
            });
    }
}
