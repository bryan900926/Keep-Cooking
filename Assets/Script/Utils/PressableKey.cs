using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PressableKey : MonoBehaviour
{
    [Header("Press Settings")]
    public float pressScaleX = 1f;      // Keep width same
    public float pressScaleY = 0.8f;    // Squash height to 80%
    public float pressDuration = 0.2f;
    public Color pressColor = Color.gray;

    [Header("Shadow Settings (Optional)")]
    public Transform shadowTransform;
    public Vector3 shadowOffset = new Vector3(0, -0.05f, 0);

    private Vector3 originalScale;
    private Color originalColor;
    private Vector3 shadowOriginalPos;
    private SpriteRenderer spriteRenderer;
    private bool isPressing = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;

        if (shadowTransform != null)
            shadowOriginalPos = shadowTransform.localPosition;
    }
    void Update()
    {
        Press();
    }

    public void Press()
    {
        if (!isPressing)
            StartCoroutine(PressCoroutine());
    }

    private System.Collections.IEnumerator PressCoroutine()
    {
        isPressing = true;

        float halfDuration = pressDuration / 2f;
        float t = 0f;

        // --- Press down smoothly ---
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float factor = t / halfDuration;

            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x, originalScale.x * pressScaleX, factor),
                Mathf.Lerp(originalScale.y, originalScale.y * pressScaleY, factor),
                originalScale.z
            );

            spriteRenderer.color = Color.Lerp(originalColor, pressColor, factor);

            if (shadowTransform != null)
                shadowTransform.localPosition = Vector3.Lerp(shadowOriginalPos, shadowOriginalPos + shadowOffset, factor);

            yield return null;
        }

        // --- Hold at pressed state ---
        yield return new WaitForSeconds(halfDuration);

        t = 0f;
        // --- Release smoothly ---
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float factor = t / halfDuration;

            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x * pressScaleX, originalScale.x, factor),
                Mathf.Lerp(originalScale.y * pressScaleY, originalScale.y, factor),
                originalScale.z
            );

            spriteRenderer.color = Color.Lerp(pressColor, originalColor, factor);

            if (shadowTransform != null)
                shadowTransform.localPosition = Vector3.Lerp(shadowOriginalPos + shadowOffset, shadowOriginalPos, factor);

            yield return null;
        }

        // --- Ensure exact final state ---
        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;
        if (shadowTransform != null)
            shadowTransform.localPosition = shadowOriginalPos;

        isPressing = false;
    }

    public bool IsPressing()
    {
        return isPressing;
    }
}
