using System.Collections.Generic;
using UnityEngine;

public class GoblinRobbingState : GoblinState
{
    private Transform targetDishTransform;

    private int tableIdx;

    public GoblinRobbingState(GoblinStateManager stateManager) : base(stateManager)
    {
    }

    public override void Enter()
    {
        FindTarget();
    }

    public override void Update()
    {
        if (Vector3.Distance(stateManager.transform.position, targetDishTransform.position) < 0.1f)
        {
            StealDish();
        }
    }

    public override void Exit()
    {
    }

    private void FindTarget()
    {
        tableIdx = Random.Range(0, DiningSystem.Instance.seats.Length);
        targetDishTransform = DiningSystem.Instance.seats[tableIdx].transform;
        stateManager.DestinationSetter.target = targetDishTransform;
    }

    private void StealDish()
    {
        if (DiningSystem.Instance.SeatToCustomer.ContainsKey(tableIdx))
        {
            GameObject customer = DiningSystem.Instance.SeatToCustomer[tableIdx];
            Holding goblinHolding = stateManager.GetComponent<Holding>();
            if (customer.TryGetComponent<CustomerStateManager>(out var customerStateManager))
            {
                Holding customerHolding = customer.GetComponent<Holding>();
                List<GameObject> customerItems = customerHolding.HoldingItem;
                foreach (var item in customerItems)
                {
                    customerHolding.RemoveHoldingItem(item);
                    goblinHolding.PickUpItem(item);
                }
                customerStateManager.IsAngry = true;
                customerStateManager.ChangeState(new CustomerLeaveState(customerStateManager));
                DiningSystem.Instance.RemoveCustomer(customer);
            }
        }

        stateManager.ChangeState(new GoblinLeaveState(stateManager));
    }
}