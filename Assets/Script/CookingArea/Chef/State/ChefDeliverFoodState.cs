using UnityEngine;

public class ChefDeliverFoodState : ChefState
{
    private int counterIndex = -1;
    private bool hasDelivered = false;

    public ChefDeliverFoodState(ChefStateManager chefStateManager) : base(chefStateManager) { }

    public override void Enter()
    {
        hasDelivered = false;
        counterIndex = -1;
    }

    public override void Update()
    {
        if (!hasDelivered)
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
        hasDelivered = false;
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
            GameObject food = chefStateManager.GetComponent<Holding>().HoldingItem[0];
            chefStateManager.GetComponent<Holding>().RemoveHoldingItem(food);
            hasDelivered = true;
            CounterManager.Instance.ChefFoodToCounter(counterIndex, food);

            // Now go back to the cooking station
            chefStateManager.Destination = chefStateManager.CookingMachine.GetComponent<CookingSpot>().GetSpot;
            chefStateManager.DestinationSetter.target = chefStateManager.Destination;
        }
    }

    private void ReturnToCookingStation()
    {
        if (Vector2.Distance(chefStateManager.transform.position, chefStateManager.Destination.position) < 0.3f)
        {
            Debug.Log("Chef returned to cooking station");
            chefStateManager.ChangeState(new ChefNormalState(chefStateManager));
        }
    }
}
