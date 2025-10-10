using UnityEngine;

public class ChefStateManager : MonoBehaviour
{
    private ChefState currentState;
    [SerializeField] private GameObject leaveTarget;
    private Cooker cooker;
    private GameObject cookingMachine;

    // public data for states
    public int CurrentDishIdx { get; set; } = -1;
    public float CookingTime { get; set; }
    public Cooker Cooker => cooker;
    public GameObject LeaveTarget => leaveTarget;

    [SerializeField] private Energy energy;

    private void Start()
    {
        cooker = GetComponent<Cooker>();

        cookingMachine = BackControl.Instance.GetCookers[cooker.CookIdx];

        ChangeState(new ChefNormalState(this));
    }

    private void Update()
    {
        if (energy.CurrentEnergy <= 0)
        {
            ChangeState(new ChefExhaustedState(this));
        }
    }

    public void ChangeState(ChefState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public float EnableCooking(int foodIdx)
    {
        if (CurrentDishIdx != -1 || energy.CurrentEnergy <= 0 || cookingMachine.GetComponent<CookingMachineStateManager>().CurrentState is not CookingMachineNormalState) return -1f;

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
            float wrongProb = Mathf.Clamp01(1 - energy.CurrentEnergy / energy.MaxEnergy);
            if (Random.value < wrongProb)
            {
                SetFireActive(true);
                CurrentDishIdx = Random.Range(0, menu.Length);
            }

            Vector2 spawnPos = (Vector2)transform.position + Vector2.right;
            Menu.Instance.SpawnForPlayer(CurrentDishIdx, spawnPos);
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
