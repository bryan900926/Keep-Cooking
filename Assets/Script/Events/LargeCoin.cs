using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LargeCoin : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [Header("Coin Settings")]
    [SerializeField] private float waitDuration = 3f;
    private float timer = 0f;
    private bool playerInRange = false;
    private bool isCollected = false;

    [Header("UI")]
    [SerializeField] private Canvas progressCanvas;   // 必須是 World Space Canvas
    [SerializeField] private Image progressImage;

    private int tipAmount = 0;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (progressCanvas != null)
        {
            progressCanvas.enabled = false;
        }

        if (progressImage != null)
            progressImage.fillAmount = 0f;
    }

    public void InitData(int tip, Transform seatTransform)
    {
        tipAmount = tip;
        transform.position = seatTransform.position + new Vector3(0, 2f, 0f);

        // UI 跟著 coin 正中間
        if (progressCanvas != null)
            progressCanvas.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0); 
    }

    void Update()
    {
        if (isCollected)
            return;

        //if (progressCanvas != null)
        //    progressCanvas.transform.position = transform.position + new Vector3(0, 1.2f, 0f);

        if (!playerInRange)
            return;

        timer += Time.deltaTime;

        if (progressCanvas != null)
            progressCanvas.enabled = true;

        if (progressImage != null)
            progressImage.fillAmount = Mathf.Clamp01(timer / waitDuration);

        // 完成倒數 → 給小費
        if (timer >= waitDuration)
        {
            isCollected = true;
            StartCoroutine(PickUpByPlayer());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG))
            return;

        playerInRange = true;
        timer = 0f;

        if (progressCanvas != null)
            progressCanvas.enabled = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG))
            return;

        playerInRange = false;
        timer = 0f;

        // UI 重置
        if (progressCanvas != null)
            progressCanvas.enabled = false;
        if (progressImage != null)
            progressImage.fillAmount = 0f;
    }

    private IEnumerator PickUpByPlayer()
    {
        UISFX.Instance.PlayMoney();
        ScoreManager.Instance.AddRevenue(tipAmount);

        // UI 消失
        if (progressCanvas != null)
            progressCanvas.enabled = false;

        // Coin 自己淡出
        float fadeDur = 0.6f;
        float t = 0f;
        Color originalColor = spriteRenderer.color;

        while (t < fadeDur)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDur);
            spriteRenderer.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );
            yield return null;
        }

        Destroy(gameObject);
    }
}



