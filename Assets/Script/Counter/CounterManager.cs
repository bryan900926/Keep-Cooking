using System.Collections.Generic;
using UnityEngine;

public class CounterManager : SeatingSystem
{
    public static CounterManager Instance;

    public int GetFoodCountOnCounter => occupiedSeats.Count;

    private readonly HashSet<int> reservedSeats = new();

    private HashSet<int> counterWithFood = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        food.transform.SetPositionAndRotation(seats[counterIndex].transform.position, seats[counterIndex].transform.rotation);

        // optional: reset local position if you want exact local alignment
        food.transform.localPosition = Vector3.zero;
        Debug.Log("Food placed on counter " + counterIndex);
        counterWithFood.Add(counterIndex);
    }

    public List<int> FetchFoodsFromCounter(int maxfetch = 1)
    {
        if (occupiedSeats.Count == 0)
            return new List<int>();
        List<int> fetchedCounterIdxs = new();
        for (int i = 0; i < seats.Length; i++)
        {
            if (counterWithFood.Contains(i) && !reservedSeats.Contains(i))
            {
                fetchedCounterIdxs.Add(i);
                reservedSeats.Add(i);
                if (fetchedCounterIdxs.Count >= maxfetch)
                {
                    break;
                }
            }
        }
        return fetchedCounterIdxs;
    }

    public GameObject RemoveFoodFromCounter(int counterIndex)
    {
        if (counterIndex >= 0 && counterIndex < seats.Length)
        {
            if (counterWithFood.Contains(counterIndex))
            {
                GameObject foodItem = seats[counterIndex].transform.GetChild(0).gameObject;
                reservedSeats.Remove(counterIndex);
                counterWithFood.Remove(counterIndex);
                Debug.Log("Food item removed from counter " + counterIndex);
                FreeSeat(counterIndex);
                return foodItem;
            }
            else
            {
                Debug.LogWarning("No food item found on counter " + counterIndex);
            }
        }
        return null;
    }
}
