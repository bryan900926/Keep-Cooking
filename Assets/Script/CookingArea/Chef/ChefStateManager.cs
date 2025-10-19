using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChefStateManager : MonoBehaviour
{
    // =======================
    // === Serialized Fields ===
    // =======================
    [Header("References")]
    [SerializeField] private WorkerData workerData;
    [SerializeField] private Energy energy;
    [SerializeField] private Transform destination;
    private GameObject leaveTarget;

    [Header("State Data")]
    [SerializeField] private int cookIdx = -2; // -2: waiting init, -1: quit job

    // =======================
    // === Private Fields ===
    // =======================
    private SpriteRenderer spriteRenderer;
    private AIDestinationSetter destinationSetter;
    private ChefState currentState;
    private GameObject cookingMachine;
    public GameObject CookingMachine => cookingMachine;

    private event Action onChefDestroyed;
    public event Action OnChefDestroyed
    {
        add { onChefDestroyed += value; }
        remove { onChefDestroyed -= value; }
    }

    // =======================
    // === Public Properties ===
    // =======================
    public AIDestinationSetter DestinationSetter
    {
        get => destinationSetter;
        set => destinationSetter = value;
    }

    public Transform Destination
    {
        get => destination;
        set => destination = value;
    }

    public GameObject LeaveTarget => leaveTarget;
    public int CookIdx
    {
        get => cookIdx;
        set => cookIdx = value;
    }

    public WorkerData WorkerData => workerData;

    public int CurrentDishIdx { get; set; } = -1;
    public float CookingTime { get; set; }

    public Energy Energy => energy;
    // =======================
    // === Unity Methods ===
    // =======================

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
    }
    public void Initialize(int cookIdx)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("spriteRenderer is null");
        }
        CookIdx = cookIdx;
        spriteRenderer.sprite = workerData.image;
        cookingMachine = BackControl.Instance.GetCookers[cookIdx];
        ChangeState(new ChefNormalState(this));
        leaveTarget = GameObject.FindGameObjectWithTag("Exit");

    }

    private void Update()
    {
        energy.UpdateEnergy(Time.deltaTime);
        if (energy.CurrentEnergy <= 0 && !(currentState is ChefExhaustedState))
        {
            ChangeState(new ChefExhaustedState(this, cookIdx));
        }
        currentState?.Update();
        ServeDrink();
    }

    // =======================
    // === State Handling ===
    // =======================
    public void ChangeState(ChefState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // =======================
    // === Cooking Logic ===
    // =======================
    public float EnableCooking(int foodIdx) // foodIdx = -2 means leftover
    {
        if (currentState is ChefFoodRottenState)
        {
            CenterMessage.Instance.ShowMessage(CenterMessage.FOOD_ROTTEN);
        }
        bool canCook = CurrentDishIdx == -1 &&
                       energy.CurrentEnergy > 0 &&
                       cookingMachine.GetComponent<CookingMachineStateManager>().CurrentState is CookingMachineNormalState &&
                       currentState is ChefNormalState;
        if (!canCook) return -1f;

        CurrentDishIdx = foodIdx;
        CookingTime = UnityEngine.Random.Range(2f, 3f);

        ChangeState(new ChefCookingState(this));
        cookingMachine.GetComponent<CookingMachineStateManager>().ChangeToCookState();
        return CookingTime;
    }

    public void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;

        if (CurrentDishIdx != -1 && CurrentDishIdx < menu.Length && cookIdx != -1)
        {

            float wrongProb = Mathf.Clamp01(1 - energy.CurrentEnergy / energy.MaxEnergy);

            if (UnityEngine.Random.value < wrongProb)
            {
                SetFireActive(true);
                CurrentDishIdx = UnityEngine.Random.Range(0, menu.Length);
            }
            else
            {
                cookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
            }

            Vector2 spawnPos = (Vector2)transform.position + Vector2.right;
            Menu.Instance.SpawnForPlayer(CurrentDishIdx, spawnPos);
        }

        CurrentDishIdx = -1;
    }

    // =======================
    // === Fire Control ===
    // =======================
    public void SetFireActive(bool active)
    {
        var machineState = cookingMachine.GetComponent<CookingMachineStateManager>();

        if (active)
            machineState.SetOneFire();
        else
            machineState.SetBackToNormal();
    }

    public bool BeenToWorkingStation()
    {
        return Vector2.Distance(cookingMachine.GetComponent<CookingSpot>().GetSpot.position, transform.position) < 1f;
    }

    public GameObject CreateLeftover()
    {
        Vector2 spawnPos = (Vector2)transform.position + Vector2.right;
        GameObject leftover = Menu.Instance.SpawnForPlayer(-2, spawnPos); // -2 for leftover
        return leftover;
    }
    public void ServeDrink()
    {
        if (Keyboard.current.eKey.isPressed && energy.IsReplenishing)
        {
            energy.Replenish(1f);
        }
        else
        {
            energy.IsReplenishing = false;
        }
    }

    void OnDestroy()
    {
        onChefDestroyed?.Invoke();
    }
}
