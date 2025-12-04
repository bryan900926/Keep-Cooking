using UnityEngine;
using UnityEngine.InputSystem;

namespace DiningArea.BeerMachine
{
    public class PlayerDetector : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        private BeerMachineStateManager stateManager;

        private bool playerInside = false;

        void Start()
        {
            stateManager = GetComponent<BeerMachineStateManager>();
        }

        void Update()
        {
            if (playerInside && Keyboard.current.eKey.wasPressedThisFrame && stateManager.CurrentState is BeerMachineNormalState)
            {
                stateManager.ChangeState(new BeerMachineFilledState(stateManager, 5f));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
                playerInside = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
                playerInside = false;
        }
    }
}
