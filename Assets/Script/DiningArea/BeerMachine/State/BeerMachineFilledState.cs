using UnityEngine;

public class BeerMachineFilledState : BeerMachineState
{
    private float fillingTime;


    private float elapsedTime = 0f;
    public BeerMachineFilledState(BeerMachineStateManager stateManager, float fillingTime) : base(stateManager)
    {
        this.fillingTime = fillingTime;
    }

    public override void Enter()
    {
        stateManager.GetComponent<Energy>().CurrentEnergy = 0;
        stateManager.GetComponent<Energy>().MaxEnergy = fillingTime;
        stateManager.FillProgressCanvasGroup.alpha = 1f;
    }

    public override void Update()
    {
        elapsedTime += Time.deltaTime;
        stateManager.GetComponent<Energy>().Replenish(Time.deltaTime);
        if (elapsedTime >= fillingTime)
        {
            Menu.Instance.SpawnForPlayer(3, stateManager.transform.position + Vector3.forward * 0.25f);
            stateManager.ChangeState(new BeerMachineNormalState(stateManager));
        }
    }

    public override void Exit()
    {
        stateManager.FillProgressCanvasGroup.alpha = 0f;
    }
}