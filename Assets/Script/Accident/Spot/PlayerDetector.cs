using UnityEngine;
using UnityEngine.InputSystem;

namespace Accident.Spot
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private GameObject hint;
        private bool playerInside = false; // track if player is in trigger

        void Start()
        {
            hint.GetComponent<SpriteRenderer>().enabled = false;
        }

        void Update()
        {
            if (playerInside && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            playerInside = true;
            ShowHint(true);

            if (other.TryGetComponent<Holding>(out Holding holding))
            {
                if (holding.HoldingItem != null)
                {
                    GameObject heldItem = holding.RemoveHolding();
                    Destroy(heldItem);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            playerInside = false;
            ShowHint(false);
        }

        private void ShowHint(bool show)
        {
            hint.GetComponent<SpriteRenderer>().enabled = show;
        }
    }
}
