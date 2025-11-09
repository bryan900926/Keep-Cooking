using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ViewEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float targetIntensity = 1f;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private bool autoRemove = true;
    [SerializeField] private float holdTime = 10f;

    private Volume volume;
    private Vignette vignette;
    private Coroutine runningCoroutine;

    private float extraHoldTime = 0f;

    void Start()
    {
        volume = GetComponent<Volume>();
        if (volume == null)
        {
            Debug.LogError("No Volume component found on this GameObject!");
            return;
        }

        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("No Vignette override found in Volume profile!");
            return;
        }

        vignette.intensity.value = 0f;
    }

    public void ApplyEffect()
    {
        if (vignette == null) return;

        // If already running, just extend the hold time
        if (runningCoroutine != null)
        {
            extraHoldTime += holdTime;
            return;
        }

        runningCoroutine = StartCoroutine(FadeVignette(0f, targetIntensity, fadeDuration));
    }

    public void RemoveEffect()
    {
        if (vignette == null) return;

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(FadeVignette(vignette.intensity.value, 0f, fadeDuration));
    }

    private IEnumerator FadeVignette(float start, float end, float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, end, elapsed / time);
            yield return null;
        }

        vignette.intensity.value = end;

        if (autoRemove && end > 0f)
        {
            float remainingTime = holdTime;

            while (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;

                // If extra time was added, extend the hold duration
                if (extraHoldTime > 0f)
                {
                    remainingTime += extraHoldTime;
                    extraHoldTime = 0f;
                }

                yield return null;
            }
            RemoveEffect();
        }
    }
}
