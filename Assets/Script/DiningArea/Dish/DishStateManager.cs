using UnityEngine;

public class DishStateManager : MonoBehaviour
{
    DishState currentState;
    void Start()
    {
        ChangeState(new DishNormalState(this));
    }


    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(DishState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
