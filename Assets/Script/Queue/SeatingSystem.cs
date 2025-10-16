using System.Collections.Generic;
using UnityEngine;

public abstract class SeatingSystem : MonoBehaviour
{
    public GameObject[] seats;
    protected HashSet<int> occupiedSeats = new HashSet<int>();

    private readonly object seatLock = new object();

    public LinkedList<int> availSeats = new LinkedList<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
    }

    public int FetchAvailSeat()
    {
        lock (seatLock)
        {
            if (availSeats.Count == 0)
                return -1;

            int seatIndex = availSeats.First.Value;
            availSeats.RemoveFirst();
            occupiedSeats.Add(seatIndex);
            return seatIndex;
        }
    }

    public virtual void FreeSeat(int seatIndex)
    {
        if (occupiedSeats.Contains(seatIndex))
        {
            occupiedSeats.Remove(seatIndex);
            availSeats.AddLast(seatIndex); // back to the queue
        }
    }
}
