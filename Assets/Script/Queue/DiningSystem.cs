using UnityEngine;

public class DiningSystem : SeatingSystem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        seats = GameObject.FindGameObjectsWithTag("DiningSeat");
        for (int i = 0; i < seats.Length; i++)
        {
            availSeats.AddLast(i);
        }

    }

    // void Update()
    // {
    //     Debug.Log("avail seat count: " + availSeats.Count);
    // }


}
