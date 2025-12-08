using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private int tipAmount = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float existingTime = 5f;

    private const string PLAYER_TAG = "Player";

    private Coroutine coroutine;

    public void InitData(int tip, Transform seatTransform)
    {
        tipAmount = tip;
        transform.position = seatTransform.position + new Vector3(0, 1.5f, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG) && coroutine == null)
        {
            coroutine = StartCoroutine(PickUpByPlayer());
        }
    }
    void Update()
    {
        existingTime -= Time.deltaTime;
        if (existingTime <= 0f && coroutine == null)
        {
            coroutine = StartCoroutine(PickUpByPlayer(false));
        }
    }

    private IEnumerator PickUpByPlayer(bool pickedByPlayer = true)
    {
        if (pickedByPlayer)
        {
            UISFX.Instance.PlayMoney();
            ScoreManager.Instance.AddRevenue(tipAmount);
        }
        float duration = 1f; // how long the fade takes
        float time = 0f;

        Color originalColor = spriteRenderer.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);

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
