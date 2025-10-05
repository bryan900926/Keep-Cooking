using UnityEngine;

public class CookingMachineStateManager : MonoBehaviour
{
    private CookingMachineState currentState;

    public CookingMachineState CurrentState => currentState;
    public GameObject fires;
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

}
