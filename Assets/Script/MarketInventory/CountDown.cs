using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timeText;   // 拖入 UI 顯示用
    public float countdownTime = 45f;  // 預設 45 秒

    private float currentTime;
    private bool isCounting = false;

    void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        currentTime = countdownTime;
        isCounting = true;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (true)
        {
            currentTime = countdownTime;

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                timeText.text = Mathf.Ceil(currentTime).ToString() + "S";
                yield return null;
            }

            timeText.text = "0S";

            MarketEventManager.Instance.TriggerRandomEvent();

        }
    }

}
