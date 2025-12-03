using UnityEngine;
using UnityEngine.InputSystem;

namespace DiningArea.Customer
{
    public class PlayerDetector : MonoBehaviour
    {

        private Energy energy;
        private const string PLAYER_TAG = "Player";
        private bool playerInside = false; // track if player is in trigger

        void Start()
        {
            energy = GetComponent<Energy>();
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                playerInside = true;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                playerInside = false;
            }
        }
        void Update()
        {
            if (playerInside && Keyboard.current.eKey.isPressed)
            {
                energy.IsReplenishing = true;
                ServeDrink();
            }
        }
        public void ServeDrink()
        {
            if (Keyboard.current.eKey.isPressed && energy.IsReplenishing)
            {
                energy.Replenish(1f);
            }
            else
            {
                energy.IsReplenishing = false;
            }
        }
    }
}

