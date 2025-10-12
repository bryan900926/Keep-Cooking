using UnityEngine;

public class CookingMachineStateManager : MonoBehaviour
{
    private CookingMachineState currentState;

    [SerializeField] private GameObject smokeEffect;

    public GameObject SmokeEffect => smokeEffect;

    public CookingMachineState CurrentState => currentState;
    [SerializeField] private GameObject fires;
    public GameObject Fires => fires;
    void Start()
    {
        ChangeState(new CookingMachineNormalState(this));
    }

    private void ChangeState(CookingMachineState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetOneFire()
    {
        ChangeState(new CookingMachineOnFireState(this));
    }

    public void SetBackToNormal()
    {
        ChangeState(new CookingMachineNormalState(this));
    }
    public void ChangeToCookState()
    {
        ChangeState(new CookingMachineCookState(this));
    }

}
