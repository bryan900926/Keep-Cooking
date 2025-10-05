using UnityEngine;

public class ChefStateManager : MonoBehaviour
{
    [Header("Cooking State")]
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;
    [SerializeField] private GameObject leaveTarget;
    private Cooker cooker;

    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyDecay = 3f;
    private float currentEnergy;

    private ChefState currentState;

    private GameObject cookingMachine;

    // public data for states
    public int CurrentDishIdx { get; set; } = -1;
    public float CookingTime { get; set; }
    public Cooker Cooker => cooker;
    public GameObject LeaveTarget => leaveTarget;

    private void Start()
    {
        currentEnergy = maxEnergy;
        cooker = GetComponent<Cooker>();

        cookingMachine = BackControl.Instance.GetCookers[cooker.CookIdx];

        ChangeState(new ChefNormalState(this));
    }

    private void Update()
    {
        currentEnergy -= Time.deltaTime * energyDecay;
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);

        if (currentEnergy <= 0)
        {
            ChangeState(new ChefExhaustedState(this));
            return;
        }

        currentState?.Update();
    }

    public void ChangeState(ChefState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public float EnableCooking(int foodIdx)
    {
        if (CurrentDishIdx != -1 || currentEnergy <= 0 || cookingMachine.GetComponent<CookingMachineStateManager>().CurrentState is not CookingMachineNormalState) return -1f;

        CurrentDishIdx = foodIdx;
        CookingTime = Random.Range(3f, 5f);

        ChangeState(new ChefCookingState(this));
        return CookingTime;
    }

    public void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;

        if (CurrentDishIdx != -1 && CurrentDishIdx < menu.Length && cooker.CookIdx != -1)
        {
            float wrongProb = Mathf.Clamp01(1 - currentEnergy / maxEnergy);
            if (Random.value < wrongProb)
            {
                SetFireActive(true);
                CurrentDishIdx = Random.Range(0, menu.Length);
            }

            Vector2 spawnPos = (Vector2)transform.position + Vector2.right;
            Instantiate(menu[CurrentDishIdx], spawnPos, Quaternion.identity);
        }

        CurrentDishIdx = -1;
    }

    public void SetFireActive(bool active)
    {
        if (active)
        {
            cookingMachine.GetComponent<CookingMachineStateManager>().SetOneFire();
        }
        else
        {
            cookingMachine.GetComponent<CookingMachineStateManager>().SetBackToNormal();
        }
    }
}
