using UnityEngine;

public class ChefDeliverFoodState : ChefState
{
    private int counterIndex = -1;
    private Holding holding;

    public ChefDeliverFoodState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        holding = chefStateManager.GetComponent<Holding>();
        counterIndex = -1;
    }

    public override void Update()
    {
        if (holding.HoldingCount > 0)
        {
            FindCounterForFood();
            DeliverToCounter();
        }
        else
        {
            ReturnToCookingStation();
        }
    }

    public override void Exit()
    {
        counterIndex = -1;
    }

    private void FindCounterForFood()
    {
        if (counterIndex != -1)
            return;

        counterIndex = CounterManager.Instance.FetchAvailSeat();
        if (counterIndex != -1)
        {
            chefStateManager.Destination = CounterManager.Instance.seats[counterIndex].transform;
            chefStateManager.DestinationSetter.target = chefStateManager.Destination;
        }
    }

    private void DeliverToCounter()
    {
        if (counterIndex != -1 && Vector2.Distance(chefStateManager.transform.position, chefStateManager.Destination.position) < 0.3f)
        {
            GameObject food = holding.HoldingItem[0];
            if (food == null)
            {
                Debug.LogWarning("No food to deliver!");
                return;
            }
            holding.RemoveHoldingItem(food);
            CounterManager.Instance.ChefFoodToCounter(counterIndex, food);
            counterIndex = -1;
        }
    }

    private void ReturnToCookingStation()
    {
        if (Vector2.Distance(chefStateManager.transform.position, chefStateManager.Destination.position) < 0.3f)
        {
            Debug.Log("Chef returned to cooking station");
            chefStateManager.Destination = null;
            chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
        }
    }
}
