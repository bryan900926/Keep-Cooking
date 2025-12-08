using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [SerializeField] private float gameDuration = 10f; // 5 minutes
    private CustomerSpawner customerSpawner;

    private float remainingTime;
    private bool First = true;
    private bool Second = true;

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
        customerSpawner = FindFirstObjectByType<CustomerSpawner>();
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

        if (GetRemainingTimeRatio() <= 0.8 && First == true)
        {   
            First = false;
            customerSpawner.spawnIntervals[0] -= 1;
            customerSpawner.spawnIntervals[1] -= 1;
        }

        if (GetRemainingTimeRatio() <= 0.4 && Second == true)
        {
            Second = false;
            customerSpawner.spawnIntervals[0] -= 1;
            customerSpawner.spawnIntervals[1] -= 1;
        }


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
        First = true;
        Second = true;
        SetTimeText();
    }
}
