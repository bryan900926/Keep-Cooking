using System.Linq;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Holding))]
public class GoblinStateManager : MonoBehaviour
{
    private GoblinState currentState;

    private static readonly string GOBLIN_IDLE_POSITION_TAG = "GoblinIdlePosition";

    private Holding holding;

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] idlePositions;

    [SerializeField] private AIDestinationSetter destinationSetter;

    [SerializeField] private AIPath aiPath;

    public Transform[] IdlePositions
    {
        get
        {
            return idlePositions;
        }
    }
    public AIDestinationSetter DestinationSetter
    {
        get
        {
            return destinationSetter;
        }
    }

    public Transform ExitPoint
    { get => exitPoint; set => exitPoint = value; }


    void Awake()
    {
        holding = GetComponent<Holding>();
        ValidateRef(exitPoint, "Exit Point");
        ValidateRef(destinationSetter, "AIDestinationSetter");
        ValidateRef(aiPath, "AIPath");
        ValidateRef(holding, "Holding");
    }

    void Start()
    {
        idlePositions = GameObject.FindGameObjectsWithTag(GOBLIN_IDLE_POSITION_TAG).Select(go => go.transform).ToArray();
        ChangeState(new GoblinIdleState(this, idlePositions));
        aiPath.maxSpeed = 3.5f; // Set the desired max speed for the goblin
    }

    public void ChangeState(GoblinState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    // Helper method to keep Awake clean
    private void ValidateRef(Object obj, string label)
    {
        //if (obj == null)
        //{
        //    Debug.LogError($"[GoblinStateManager] Missing Reference: {label} on {name}", this);
        //    enabled = false; // Stop Update() from running to prevent crashes
        //}
    }
}