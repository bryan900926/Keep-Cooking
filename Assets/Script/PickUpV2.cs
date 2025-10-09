using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpV2 : MonoBehaviour
{
    private string PICK_UP_TAG = "Player";  // Tag of the player object
    private Vector3 offset = new Vector3(0, 2f, 0); // Position above player

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
        if (Keyboard.current.eKey.wasPressedThisFrame && pickable)
        {
            if (!pickedUp && inRange && playerInRange != null && !playerInRange.GetComponent<Holding>().HoldingItem)
            {
                Pick(playerInRange.gameObject);
            }
            else if (pickedUp)
            {
                Drop();

            }
        }
    }

    public void Pick(GameObject picker)
    {
        // Pick up
        pickedUp = true;
        if (picker.GetComponent<Holding>().HoldingItem) return;

        transform.SetParent(picker.transform);
        picker.GetComponent<Holding>().SetHoldingItem(gameObject);
        transform.localPosition = offset;
        currentHolder = picker.transform;

        Debug.Log("Picked up: " + gameObject.name);
    }

    public void Drop()
    {
        // Drop
        pickedUp = false;
        transform.SetParent(null);
        currentHolder.GetComponent<Holding>().RemoveHolding();
        currentHolder  = null;
        Debug.Log("Dropped: " + gameObject.name);
    }
}
