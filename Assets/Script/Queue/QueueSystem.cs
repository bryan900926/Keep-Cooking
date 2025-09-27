using System.Collections.Generic;
using UnityEngine;

public class QueueSystem : SeatingSystem
{
    [Header("Queue Settings")]
    public int numberOfSlots = 5;     // how many queue positions
    public float spacing = 1.5f;      // distance between each slot
    public Vector3 direction = Vector3.left; // direction of the queue

    [Header("Slot Visuals (Optional)")]
    public GameObject slotPrefab; // leave empty if you just want empties

    protected override void Start()
    {
        seats = new GameObject[numberOfSlots];
        GenerateSlots();
    }

    void GenerateSlots()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            // Calculate position
            Vector2 pos = (Vector2)transform.position + (Vector2)direction.normalized * spacing * i;


            // Create optional visual
            if (slotPrefab != null)
            {
                GameObject seat = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
                seats[i] = seat;
                availSeats.AddLast(i);
            }
        }
    }
}
