using UnityEngine;

namespace CookingArea.CookingMachine
{
    public class PlayerDetector : MonoBehaviour
    {
        private CookingMachineStateManager stateManager;

        private const string PLAYER_TAG = "Player";

        void Start()
        {
            stateManager = GetComponent<CookingMachineStateManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (stateManager.CurrentState is CookingMachineOnFireState && other.CompareTag(PLAYER_TAG))
            {
                stateManager.SetBackToNormal();
            }
        }
    }

}
