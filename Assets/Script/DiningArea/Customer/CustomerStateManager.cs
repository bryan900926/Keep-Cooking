using Pathfinding;
using UnityEngine;

public class CustomerStateManager : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;

    public WorkerData WorkerData { get => workerData; set => workerData = value; }
    private QueueSystem queueSystem;
    public QueueSystem QueueSystem => queueSystem;
    private GameObject queueObj;

    private GameObject diningObj;

    private DiningSystem diningSystem;

    public DiningSystem DiningSystem => diningSystem;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = workerData.image;
        queueObj = GameObject.FindGameObjectWithTag("Queue");
        diningObj = GameObject.FindGameObjectWithTag("Dining");
        diningSystem = diningObj.GetComponent<DiningSystem>();
        queueSystem = queueObj.GetComponent<QueueSystem>();
        currentState = new CustomerWaitLineState(this);
        viewEffect = GameObject.FindGameObjectWithTag("PostProcess").GetComponent<ViewEffect>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        if (orderedFoodIdx != -1)
        {
            energy.UpdateEnergy(Time.deltaTime);

        }
        if (energy.CurrentEnergy <= 0 && !(currentState is CustomerToChefState))
        {
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
