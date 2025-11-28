using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [SerializeField] private float gameDuration = 10f; // 5 minutes

    private float remainingTime;

    public float RemainingTime => remainingTime;
    private TextMeshProUGUI timerText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        remainingTime = gameDuration;
        SetTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        SetTimeText();

    }

    private void SetTimeText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
        timerText.SetText(string.Format("{0:0}:{1:00}", minutes, seconds));
    }

    public float GetRemainingTimeRatio()
    {
        return remainingTime / gameDuration;
    }

    public void ResetTimer()
    {
        remainingTime = gameDuration;
        SetTimeText();
    }
}
