using System.Collections;
using Pathfinding;
using UnityEngine;

public class CustomerStateManager : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;
    public WorkerData WorkerData { get => workerData; set => workerData = value; }
    private QueueSystem queueSystem;
    public QueueSystem QueueSystem => queueSystem;
    private GameObject queueObj;

    [SerializeField] private Sprite emoji_great;
    [SerializeField] private Sprite emoji_good;
    [SerializeField] private Sprite emoji_bad;
    [SerializeField] private Sprite emoji_terrible;

    // Customer Propertis

    // ------ Begin ------
    private float[] buyingPrice;
    public float[] BuyingPrice { get => buyingPrice; set => buyingPrice = value; }
    private float eatingDuration;
    public float EatingDuration { get => eatingDuration; set => eatingDuration = value; }
    private float tipsratio;
    public float Tipsratio { get => tipsratio; set => tipsratio = value; }
    private float addreputation;
    public float Addreputation { get => addreputation; set => addreputation = value; }
    private float minusreputation;
    public float Minusreputation { get => minusreputation; set => minusreputation = value; }

    private float maxspeed;
    public float Maxspeed { get => maxspeed; set => maxspeed = value; }

    public CustomerProperty customerProperty;
    // ------ End ------

    public float sellprice;

    public Customrep feedbackUI; // Customer's current expression

    private static readonly Color feedbackTextColor = new Color(0.15f, 0.15f, 0.15f);

    private bool leave = false;

    private DiningSystem diningSystem;


    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;

    private int liningIdx = -1;
    private int diningIdx = -1;
    private int orderedFoodIdx = -1;

    public int LiningIdx { get => liningIdx; set => liningIdx = value; }
    public int DiningIdx { get => diningIdx; set => diningIdx = value; }
    public int OrderedFoodIdx { get => orderedFoodIdx; set => orderedFoodIdx = value; }

    public AIDestinationSetter DestinationSetter => destinationSetter;
    public AIPath AiPath => aiPath;

    private CustomerState currentState;
    public CustomerState CurrentState => currentState;

    [SerializeField] private Energy energy;
    public Energy Energy { get => energy; set => energy = value; }
    private ViewEffect viewEffect;
    public ViewEffect ViewEffect => viewEffect;

    private CustomerSFX customerSFX;
    public CustomerSFX CustomerSFX { get => customerSFX; set => customerSFX = value; }

    [SerializeField] private GameObject coinPrefab;
    public GameObject CoinPrefab { get => coinPrefab; set => coinPrefab = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = workerData.image;
        feedbackUI = GetComponent<Customrep>();
        queueObj = GameObject.FindGameObjectWithTag("Queue");
        diningSystem = DiningSystem.Instance;
        queueSystem = queueObj.GetComponent<QueueSystem>();
        currentState = new CustomerWaitLineState(this);
        viewEffect = GameObject.FindGameObjectWithTag("PostProcess").GetComponent<ViewEffect>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        customerSFX = GetComponent<CustomerSFX>();
        aiPath.maxSpeed = 4f;
        currentState.Enter();
    }

    public void Attributeprop(int index)
    {
        CustomerProperty customerProperty = CustomerPropertyManager.Instance.customerProperties[index];
        Debug.Log($"Attribute Property for Customer Type: {customerProperty.truevalue.Length}");
        BuyingPrice = (float[])customerProperty.truevalue.Clone();
        EatingDuration = customerProperty.eatingDuration;
        Tipsratio = customerProperty.tipsratio;
        Addreputation = customerProperty.addreputation;
        Minusreputation = customerProperty.minusreputation;
        maxspeed = customerProperty.maxspeed;
        aiPath = GetComponent<AIPath>();
        Debug.Log("aiPath is: " + aiPath);
        aiPath.maxSpeed = maxspeed;
    }

    public void ReactBad()
    {
        feedbackUI.ShowFeedback(emoji_bad, "Kinda pricey...", feedbackTextColor); // Orange color
    }

    public void ReactTerrible()
    {
        feedbackUI.ShowFeedback(emoji_terrible, "Way too much!", feedbackTextColor);
    }

    public void ReactGood()
    {
        feedbackUI.ShowFeedback(emoji_good, "Nice deal!", feedbackTextColor);
    }

    public void ReactGreat()
    {
        feedbackUI.ShowFeedback(emoji_great, "Sweet bargain!", feedbackTextColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (orderedFoodIdx != -1 && currentState is CustomerWaitFoodState)
        {
            energy.UpdateEnergy(Time.deltaTime);

        }
        if (energy.CurrentEnergy <= 0 && currentState is not CustomerToChefState)
        {
            if (!leave)
            {
                leave = true;
                ReputationSystem.Instance.DecreaseReputation(minusreputation);
                CustomerPropertyManager.Instance.Addsatisfactory(customerProperty, -1);
                CustomerPropertyManager.Instance.Updateprop(customerProperty);
            }
            ChangeState(new CustomerLeaveState(this));
        }
        currentState?.Update();
    }

    public void ChangeState(CustomerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
