using System.Collections.Generic;
using UnityEngine;

public class DiningSystem : SeatingSystem
{

    public static DiningSystem Instance { get; private set; }

    private Dictionary<int, GameObject> seatToCustomer = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> SeatToCustomer { get { return seatToCustomer; } set { seatToCustomer = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    protected override void Start()
    {
        seats = GameObject.FindGameObjectsWithTag("DiningSeat");
        for (int i = 0; i < seats.Length; i++)
        {
            GameObject seat = seats[i];
            seat.GetComponent<Seat>().SetSeatNumber(i);
            availSeats.AddLast(i);
        }
        // Debug.Log("Dining system initialized with " + availSeats.Count + " seats.");

    }

    public GameObject GetCustomerAtSeat(int idx)
    {
        if (seatToCustomer.ContainsKey(idx))
        {
            return seatToCustomer[idx];
        }
        else
        {
            return null;
        }
    }

    public override void FreeSeat(int seatIndex)
    {
        if (seatToCustomer.ContainsKey(seatIndex))
        {
            occupiedSeats.Remove(seatIndex);
            seatToCustomer.Remove(seatIndex);
            availSeats.AddLast(seatIndex); // back to the queue
        }
    }


}
