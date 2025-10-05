using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpV2 : MonoBehaviour
{
    public string pickupTag = "Player";  // Tag of the player object
    public Vector3 offset = new Vector3(0, 2f, 0); // Position above player

    private bool pickedUp = false;
    private Transform player;
    private bool inRange = false;

    public int foodIdx = -1;
    private Transform playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(pickupTag))
        {
            inRange = true;
            playerInRange = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(pickupTag))
        {
            inRange = false;
            playerInRange = null;
        }
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("pickup: " + pickedUp + " inRange: " + inRange);
            if (!pickedUp && inRange)
            {
                // Pick up
                pickedUp = true;
                player = playerInRange;
                if (player.GetComponent<Holding>().HoldingItem) return;

                transform.SetParent(player);
                player.GetComponent<Holding>().SetHoldingItem(gameObject);
                transform.localPosition = offset;

                Debug.Log("Picked up: " + gameObject.name);
            }
            else if (pickedUp)
            {
                // Drop
                pickedUp = false;
                transform.SetParent(null);
                player.GetComponent<Holding>().RemoveHolding();
                player = null;
                Debug.Log("Dropped: " + gameObject.name);
            }
        }
    }
}
