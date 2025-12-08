using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpV2 : MonoBehaviour
{
    private string PICK_UP_TAG = "Player";  // Tag of the player object
    private Vector3 offset = new(0, 2f, 0); // Position above player

    private bool pickedUp = false;
    private bool inRange = false;

    private Transform currentHolder;

    private int foodIdx = -1;
    public int FoodIdx { get { return foodIdx; } set { foodIdx = value; } }

    private Transform playerInRange;

    [SerializeField] private bool pickable = true;
    public bool Pickable { get { return pickable; } set { pickable = value; } }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PICK_UP_TAG))
        {
            inRange = true;
            playerInRange = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(PICK_UP_TAG))
        {
            inRange = false;
            playerInRange = null;
        }
    }

    void Update()
    {
        if (pickable)
        {
            if (!pickedUp && inRange && playerInRange != null && playerInRange.GetComponent<Holding>().HasSpace())
            {
                Pick(playerInRange.gameObject);
            }
        }
    }

    public void Pick(GameObject picker)
    {
        // Pick up
        pickedUp = true;
        if (!picker.GetComponent<Holding>().HasSpace()) return;
        picker.GetComponent<Holding>().PickUpItem(gameObject);
        currentHolder = picker.transform;
    }
}
