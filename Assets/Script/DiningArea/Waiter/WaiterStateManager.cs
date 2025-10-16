using Pathfinding;
using UnityEngine;

public class WaiterStateManager : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;
    public WorkerData WorkerData => workerData;
    [SerializeField] private WaiterState currentState;

    public WaiterState CurrentState => currentState;
    public int foodIdx = -1;
    public int tableIdx = -1;

    public AIDestinationSetter destinationSetter;


    private DiningSystem diningSystem;
    public AIPath aiPath;

    private Transform waitingSpot;

    private SpriteRenderer spriteRenderer;


    void Start()
    {
        diningSystem = DiningSystem.Instance;
        waitingSpot = GameObject.FindGameObjectWithTag("WaiterWaiting").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (workerData != null)
        {
            spriteRenderer.sprite = workerData.image;
        }
        else
        {
            Debug.LogWarning($"{name} has no WorkerData assigned!");
        }
        currentState = new WaiterIdleState(this);
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(WaiterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public Transform GetWaitingSpot()
    {
        return waitingSpot;
    }
}
