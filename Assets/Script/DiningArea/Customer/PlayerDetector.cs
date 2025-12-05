using UnityEngine;
using UnityEngine.InputSystem;

namespace DiningArea.Customer
{
    public class PlayerDetector : MonoBehaviour
    {

        private Energy energy;
        private const string PLAYER_TAG = "Player";
        private Holding playerHolding; // track if player is in trigger

        void Start()
        {
            energy = GetComponent<Energy>();
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                playerHolding = other.GetComponent<Holding>();
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                playerHolding = null;
            }
        }
        void Update()
        {
            if (playerHolding != null && Keyboard.current.eKey.isPressed)
            {
                if (playerHolding.FindBeer())
                {
                    energy.FeedDrink();
                }
            }
        }

    }
}

