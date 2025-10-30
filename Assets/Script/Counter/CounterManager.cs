using UnityEngine;

public class CounterManager : SeatingSystem
{
    public static CounterManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Start()
    {
        for (int i = 0; i < seats.Length; i++)
        {
            availSeats.AddLast(i);
        }


    }

    public void ChefFoodToCounter(int counterIndex, GameObject food)
    {
        // set as child
        food.transform.SetParent(seats[counterIndex].transform, false);

        // align position and rotation with the seat
        food.transform.position = seats[counterIndex].transform.position;
        food.transform.rotation = seats[counterIndex].transform.rotation;

        // optional: reset local position if you want exact local alignment
        food.transform.localPosition = Vector3.zero;
    }

    public int FetchFoodFromCounter()
    {
        for (int i = 0; i < seats.Length; i++)
        {
            if (occupiedSeats.Contains(i) && seats[i].transform.childCount > 0)
            {
                occupiedSeats.Remove(i);
                availSeats.AddLast(i);
                return i;
            }
        }
        return -1;
    }

    public void RemoveFoodFromCounter(int counterIndex, WaiterStateManager waiterStateManager)
    {
        if (counterIndex >= 0 && counterIndex < seats.Length)
        {
            if (seats[counterIndex].transform.childCount > 0)
            {
                GameObject foodItem = seats[counterIndex].transform.GetChild(0).gameObject;
                foodItem.GetComponent<PickUpV2>().Pick(waiterStateManager.gameObject);
                foodItem.GetComponent<PickUpV2>().Pickable = false;
                waiterStateManager.foodIdx = foodItem.GetComponent<PickUpV2>().FoodIdx;
            }
        }
    }

}
