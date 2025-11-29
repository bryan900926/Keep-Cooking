using System.Collections;
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

    private Coroutine speedRoutine;
    readonly private float normalSpeed = 4f;

    private float slowedSpeed = 1f;      // lowest allowed speed
    private float recoverTimer = 0f;     // counts elapsed recovery time
    private float recoverDuration = 2f;  // how long until fully recovered

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
        aiPath.maxSpeed = normalSpeed;
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

    public void SlowDown(float slowAmount = 2f, float extraRecover = 1f)
    {
        // Apply additional slowdown
        aiPath.maxSpeed = Mathf.Max(aiPath.maxSpeed - slowAmount, slowedSpeed);

        // Each new hit extends how long recovery needs
        recoverDuration += extraRecover;

        // Restart the timer so we restart "recovery interval"
        recoverTimer = 0f;

        speedRoutine ??= StartCoroutine(RecoverSpeedRoutine());
    }
    private IEnumerator RecoverSpeedRoutine()
    {
        float startSpeed = aiPath.maxSpeed;

        while (recoverTimer < recoverDuration)
        {
            recoverTimer += Time.deltaTime;

            float t = recoverTimer / recoverDuration;
            aiPath.maxSpeed = Mathf.Lerp(startSpeed, normalSpeed, t);

            yield return null;
        }

        aiPath.maxSpeed = normalSpeed;

        // Reset for next time
        recoverDuration = 2f;  // default recovery time back
        recoverTimer = 0f;
        speedRoutine = null;
    }


}
