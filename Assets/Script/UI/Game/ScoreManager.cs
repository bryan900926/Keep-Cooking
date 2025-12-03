using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private float revenue = 1000f;

    public float Revenue => revenue;

    private TextMeshProUGUI revenueText;

    [SerializeField] private TextMeshProUGUI revenueTextInInventory; // assign in inspector

    readonly private PriorityQueue<Loan> loanQueue = new();

    [SerializeField] private float interestRate = 0.01f; // 1% interest rate

    private int servedCnt = 0;

    public int ServedCnt => servedCnt;

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
        if (revenue < 0)
        {
            loanQueue.Enqueue(new Loan(revenue, interestRate), Time.time);
        } else
        {
            servedCnt++;
        }
        while (loanQueue.Count > 0 && revenue > 0)
        {
            Loan loan = loanQueue.Peek();
            bool cleared = loan.Pay(revenue);
            revenue -= Mathf.Min(revenue, loan.Principal);
            if (cleared)
            {
                loanQueue.Dequeue();
            }
            else
            {
                break;
            }
        }
        UpdateRevenueText();
    }
    private void UpdateRevenueText()
    {
        revenueText.SetText("$" + revenue.ToString());
        if (revenueTextInInventory != null)
        {
            revenueTextInInventory.SetText("$" + revenue.ToString());
        }
    }
}

public class Loan
{
    public float Principal { get; private set; }
    public float InterestRate { get; private set; }
    private float lastUpdateTime;

    public Loan(float principal, float interestRate)
    {
        Principal = principal;
        InterestRate = interestRate;
        lastUpdateTime = Time.time;
    }

    private void UpdateLoan()
    {
        float now = Time.time;
        float dt = now - lastUpdateTime;
        lastUpdateTime = now;

        // Continuous compounding
        Principal *= Mathf.Exp(InterestRate * dt);
    }

    public bool Pay(float amount)
    {
        UpdateLoan();
        float paid = Mathf.Min(amount, Principal);
        Principal -= paid;
        return IsCleared;
    }

    public bool IsCleared => Principal <= 0.01f;
}
