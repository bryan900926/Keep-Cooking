using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(ChefRecipe))]
[RequireComponent(typeof(Holding))]
public class ChefStateManager : MonoBehaviour
{
    // =======================
    // === Serialized Fields ===
    // =======================
    [Header("References")]
    [SerializeField] private WorkerData workerData;
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

    public ChefRecipe ChefRecipe => chefRecipe;

    private Holding holding;

    public Holding Holding => holding;

    [SerializeField] private SpriteRenderer lowStockSprite;

    private Coroutine flickerRoutine;

    private ChefSFX chefSFX;

    public ChefSFX ChefSFX
    {
        get => chefSFX;
        set => chefSFX = value;
    }

    [SerializeField] private GameObject sweatEffect;

    public GameObject SweatEffect
    {
        get => sweatEffect;
        set => sweatEffect = value;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        chefRecipe = GetComponent<ChefRecipe>();
        holding = GetComponent<Holding>();
        chefSFX = GetComponent<ChefSFX>();
    }

    void Start()
    {
        if (chefRecipe != null)
        {
            chefRecipe.OnRecipeChanged += SetChefHasCorrectRecipe;
        }
        lowStockSprite.enabled = false;
        sweatEffect.SetActive(false);
    }
    public void Initialize(int cookIdx)
    {
        CookIdx = cookIdx;
        spriteRenderer.sprite = workerData.image;
        cookingMachine = BackControl.Instance.GetCookers[cookIdx];
        ChangeState(new ChefNormalState(this));
        leaveTarget = GameObject.FindGameObjectWithTag("Exit");

    }

    private void Update()
    {
        currentState?.Update();
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
        bool canCook = cookingMachine.GetComponent<CookingMachineStateManager>().CurrentState is CookingMachineNormalState &&
                       currentState is ChefNormalState;
        if (!canCook) return;
        List<OrderInfo> orderInfos = OrderSystem.Instance.GetOrderForChef(Holding.AvailableSpace);
        if (orderInfos.Count == 0) return;
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
    public bool CheckChefForgetRecipe()
    {
        return UnityEngine.Random.value < 0.06f;
    }
    public void SetChefHasCorrectRecipe(bool isCorrect)
    {
        chefHasCorrectRecipe = isCorrect;
    }

    public void HandleLowStockEffect(bool isLowStock)
    {
        lowStockSprite.enabled = isLowStock;
    }
    private void OnDestroy()
    {
        if (chefRecipe != null)
            chefRecipe.OnRecipeChanged -= SetChefHasCorrectRecipe;
        onChefDestroyed?.Invoke();
    }
}
