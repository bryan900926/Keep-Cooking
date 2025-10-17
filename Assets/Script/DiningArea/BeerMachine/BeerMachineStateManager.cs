using UnityEngine;

public class BeerMachineStateManager : MonoBehaviour
{
    private BeerMachineState currentState;
    [SerializeField] private CanvasGroup fillProgressCanvasGroup;

    public CanvasGroup FillProgressCanvasGroup {
        get => fillProgressCanvasGroup;
        set => fillProgressCanvasGroup = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = new BeerMachineNormalState(this);
        currentState.Enter();

    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();

    }

    public void ChangeState(BeerMachineState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
