using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image ProgressImage;

    public Image GetProgressImage => ProgressImage;

    [SerializeField]
    private UnityEvent<float> OnProgress;
    [SerializeField]
    private UnityEvent OnCompleted;

    private Coroutine AnimationCoroutine;

    private void Start()
    {
        if (ProgressImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s ProgressImage is not of type \"Filled\" so it cannot be used as a progress bar. Disabling this Progress Bar.");
            enabled = false;
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(this.gameObject);
#endif
        }
    }

    /// <summary>
    /// Start the progress bar as a timer (0 → 1) over a specified duration
    /// </summary>
    public void StartTimer(float duration)
    {
        if (AnimationCoroutine != null)
            StopCoroutine(AnimationCoroutine);

        AnimationCoroutine = StartCoroutine(TimerRoutine(duration));
    }

    private IEnumerator TimerRoutine(float duration)
    {
        float elapsed = 0f;
        ProgressImage.fillAmount = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            ProgressImage.fillAmount = progress;

            OnProgress?.Invoke(progress);
            yield return null;
        }

        ProgressImage.fillAmount = 1f;
        OnProgress?.Invoke(1f);
        OnCompleted?.Invoke();
    }

    /// <summary>
    /// Optional: Reset the progress bar to 0 instantly
    /// </summary>
    public void ResetProgress()
    {
        if (AnimationCoroutine != null)
            StopCoroutine(AnimationCoroutine);

        ProgressImage.fillAmount = 0f;
        OnProgress?.Invoke(0f);
    }
}
