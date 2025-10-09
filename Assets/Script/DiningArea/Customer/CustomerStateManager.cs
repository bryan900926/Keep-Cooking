using Pathfinding;
using UnityEngine;

public class CustomerStateManager : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;

    public WorkerData WorkerData { get => workerData; set => workerData = value; }
    public QueueSystem queueSystem;
    private GameObject queueObj;

    private GameObject diningObj;

    public DiningSystem diningSystem;

    public AIDestinationSetter destinationSetter;
    public AIPath aiPath;

    private int liningIdx = -1;
    private int diningIdx = -1;
    private int orderedFoodIdx = -1;

    public int LiningIdx { get => liningIdx; set => liningIdx = value; }
    public int DiningIdx { get => diningIdx; set => diningIdx = value; }
    public int OrderedFoodIdx { get => orderedFoodIdx; set => orderedFoodIdx = value; }

    public CustomerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = workerData.image;
        queueObj = GameObject.FindGameObjectWithTag("Queue");
        diningObj = GameObject.FindGameObjectWithTag("Dining");
        diningSystem = diningObj.GetComponent<DiningSystem>();
        queueSystem = queueObj.GetComponent<QueueSystem>();
        currentState = new CustomerWaitLineState(this);
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(CustomerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
