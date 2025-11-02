using System.Collections.Generic;
using UnityEngine;

public class CounterManager : SeatingSystem
{
    public static CounterManager Instance;

    private HashSet<int> counterHoldingFood = new();
    public int GetFoodCountOnCounter => counterHoldingFood.Count;

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
        counterHoldingFood.Add(counterIndex);
    }

    public List<int> FetchFoodsFromCounter(int maxfetch = 1)
    {
        List<int> fetchedCounterIdxs = new();
        for (int i = 0; i < seats.Length; i++)
        {
            if (occupiedSeats.Contains(i) && seats[i].transform.childCount > 0)
            {
                occupiedSeats.Remove(i);
                availSeats.AddLast(i);
                fetchedCounterIdxs.Add(i);
                if (fetchedCounterIdxs.Count >= maxfetch)
                    break;
            }
        }
        return fetchedCounterIdxs;
    }

    public GameObject RemoveFoodFromCounter(int counterIndex)
    {
        if (counterIndex >= 0 && counterIndex < seats.Length)
        {
            if (seats[counterIndex].transform.childCount > 0)
            {
                GameObject foodItem = seats[counterIndex].transform.GetChild(0).gameObject;

                return foodItem;
            }
        }
        return null;
    }

}
