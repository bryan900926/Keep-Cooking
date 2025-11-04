using Pathfinding;
using UnityEngine;

public class WaiterStateManager : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;
    public WorkerData WorkerData => workerData;
    [SerializeField] private WaiterState currentState;

    public WaiterState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private Holding holding;

    public Holding Holding => holding;
    public int tableIdx = -1;

    public AIDestinationSetter destinationSetter;


    private DiningSystem diningSystem;
    public AIPath aiPath;

    private WaiterStandby waiterStandby;

    public WaiterStandby WaiterStandby
    {
        get { return waiterStandby; }
        set { waiterStandby = value; }
    }
    private SpriteRenderer spriteRenderer;

    private int standbySeatIdx = -1;

    void Start()
    {
        diningSystem = DiningSystem.Instance;
        waiterStandby = GameObject.FindGameObjectWithTag("WaiterWaiting").GetComponent<WaiterStandby>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        holding = GetComponent<Holding>();

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

    public void FindStandbySpot()
    {
        if (standbySeatIdx == -1)
        {
            standbySeatIdx = waiterStandby.FetchAvailSeat();
            if (standbySeatIdx == -1)
            {
                Debug.LogError("No available standby spot for waiter!");
                return;
            }
        }
        Debug.Log("Waiter " + gameObject.GetInstanceID() + " assigned to standby spot index: " + standbySeatIdx);
        destinationSetter.target = waiterStandby.seats[standbySeatIdx].transform;
    }

    public void ClearStandbySpot()
    {
        if (standbySeatIdx == -1)
        {
            Debug.LogError("Standby seat index is invalid!");
            return;
        }
        waiterStandby.FreeSeat(seatIndex: standbySeatIdx);
        standbySeatIdx = -1;
    }
}
