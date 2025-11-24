using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timeText;   // ��J UI ��ܥ�
    public float countdownTime = 45f;  // �w�] 45 ��

    private float currentTime;

    void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
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
