using System;
using System.Collections.Generic;
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
    [SerializeReference]
    public ChefState currentState;

    // =======================
    // === Private Fields ===
    // =======================
    private SpriteRenderer spriteRenderer;
    private AIDestinationSetter destinationSetter;
    private GameObject cookingMachine;
    public GameObject CookingMachine => cookingMachine;

    private ChefRecipe chefRecipe;

    private event Action onChefDestroyed;

    private bool chefHasCorrectRecipe = true;

    public bool ChefHasCorrectRecipe => chefHasCorrectRecipe;

    private float flickerTimer = 0f;
    private bool isRed = false;
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

    public List<int> currentDishIdxs = new();
    public List<int> CurrentDishIdxs { get => currentDishIdxs; set => currentDishIdxs = value; }
    public float CookingTime { get; set; }

    public Energy Energy => energy;

    public ChefRecipe ChefRecipe => chefRecipe;

    private Holding holding;

    public Holding Holding => holding;



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        chefRecipe = GetComponent<ChefRecipe>();
        holding = GetComponent<Holding>();
    }

    void Start()
    {
        if (chefRecipe != null)
        {
            chefRecipe.OnRecipeChanged += SetChefHasCorrectRecipe;
        }
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
        if (energy.CurrentEnergy <= 0 && currentState is not ChefExhaustedState)
        {
            ChangeState(new ChefExhaustedState(this, cookIdx));
        }
        currentState?.Update();
        ServeDrink();
        HandleRecipeFlicker();
    }

    public void ChangeState(ChefState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void EnableCookingManyFoods()
    {
        if (gameObject == null || cookingMachine == null) return;
        bool canCook = energy.CurrentEnergy > 0 &&
                       cookingMachine.GetComponent<CookingMachineStateManager>().CurrentState is CookingMachineNormalState &&
                       currentState is ChefNormalState;
        if (!canCook) return;
        List<OrderInfo> orderInfos = OrderSystem.Instance.GetOrderForChef(Holding.AvailableSpace);
        if (orderInfos == null || orderInfos.Count == 0) return;
        if (CheckChefForgetRecipe())
        {
            ChangeState(new ChefForgetState(this, orderInfos));
        }
        else
        {
            ChangeState(new ChefCookingState(this, orderInfos));
        }

    }

    public void SetFireActive(bool active)
    {
        var machineState = cookingMachine.GetComponent<CookingMachineStateManager>();

        if (active)
            machineState.SetOneFire();
        else
            machineState.SetBackToNormal();
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
    public bool CheckChefForgetRecipe()
    {
        // return 0.08 > UnityEngine.Random.value;
        return true;
    }
    public void SetChefHasCorrectRecipe(bool isCorrect)
    {
        chefHasCorrectRecipe = isCorrect;
    }

    private void HandleRecipeFlicker()
    {
        if (!chefHasCorrectRecipe)
        {
            flickerTimer += Time.deltaTime;

            if (flickerTimer >= 0.2f) // flicker every 0.2s
            {
                flickerTimer = 0f;
                isRed = !isRed;
                spriteRenderer.color = isRed ? Color.red : Color.white;
            }
        }
        else
        {
            // reset to normal color
            if (spriteRenderer.color != Color.white)
                spriteRenderer.color = Color.white;

            flickerTimer = 0f;
            isRed = false;
        }
    }

    private void OnDestroy()
    {
        if (chefRecipe != null)
            chefRecipe.OnRecipeChanged -= SetChefHasCorrectRecipe;
        onChefDestroyed?.Invoke();
    }
}
