using UnityEngine;

public class ReceiptSystem : SeatingSystem
{

    public static ReceiptSystem Instance { get; private set; }
    [Header("Queue Settings")]
    [SerializeField] private int numberOfSlots = 1;     // how many queue positions
    [SerializeField] private float spacing = 1.5f;      // distance between each slot
    [SerializeField] private Vector3 direction = Vector3.left; // direction of the queue

    [Header("Slot Visuals (Optional)")]
    [SerializeField] private GameObject slotPrefab; // leave empty if you just want empties

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    protected override void Start()
    {
        seats = new GameObject[numberOfSlots];
        GenerateSlots();
    }

    private void GenerateSlots()
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
