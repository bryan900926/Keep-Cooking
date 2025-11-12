using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private float revenue = 0f;

    private TextMeshProUGUI revenueText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        revenueText = GetComponent<TextMeshProUGUI>();
        UpdateRevenueText();
    }
    public void AddRevenue(float amount)
    {
        revenue += amount;
        UpdateRevenueText();
    }
    private void UpdateRevenueText()
    {
        revenueText.SetText("$" + revenue.ToString());
    }
}
