using System.Collections;
using UnityEngine;

public class DishEatenState : DishState
{
    private float eatingDuration = 2f;
    public DishEatenState(DishStateManager dishStateManager, float eatingDuration) : base(dishStateManager)
    {
        this.eatingDuration = eatingDuration;
    }

    public override void Enter()
    {
        StartEating(eatingDuration);
    }

    public void StartEating(float eatingDuration)
    {
        var coroutine = EatCoroutine(eatingDuration);
        dishStateManager.StartCoroutine(coroutine);
    }


    private IEnumerator EatCoroutine(float eatingDuration)
    {
        Vector3 originalScale = dishStateManager.transform.localScale;
        float t = 0f;

        while (t < eatingDuration)
        {
            t += Time.deltaTime;
            float factor = 1f - t / eatingDuration;
            dishStateManager.transform.localScale = originalScale * factor;
            yield return null;
        }

        // Ensure food disappears completely
        dishStateManager.transform.localScale = Vector3.zero;
        Object.Destroy(dishStateManager.gameObject);
    }
}