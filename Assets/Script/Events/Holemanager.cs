using System.Collections;
using DG.Tweening;
using UnityEngine;


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

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }
    public void HoleGenerator(int index)
    {
        var dining = DiningSystem.Instance;
        Vector3 pos = dining.seats[index].transform.position;
        Vector3 holePos = pos + new Vector3(0f, -0.4f, 0f);
        GameObject Hole = Instantiate(HolePrefab, holePos, Quaternion.identity);
        Hole.GetComponent<Hole>().HoleAction();
    }

    public void TriggerHole(GameObject customer)
    {
        PlayHoleAnimation(customer);
    }

    private void PlayHoleAnimation(GameObject customer)
    {
        if (customer == null) return;

        customer.transform.DOMoveY(customer.transform.position.y - 0.5f, 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                if (customer.TryGetComponent<CustomerStateManager>(out var customerStateManager))
                {
                    customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
                }
                DOTween.Kill(customer.transform, false);
                customer.GetComponentInChildren<SpriteRenderer>().enabled = false;
                Destroy(customer, 0.5f);
            });
    }
}
