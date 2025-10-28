using UnityEngine;

namespace DiningArea.Waiter
{
    public class PlayerDetector : MonoBehaviour
    {
        private WaiterStateManager stateManager;

        private const string PLAYER_TAG = "Player";

        void Start()
        {
            stateManager = GetComponent<WaiterStateManager>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (stateManager.CurrentState is WaiterIdleState && other.CompareTag(PLAYER_TAG))
            {
                GameObject foodItem = other.GetComponent<Holding>().HoldingItem;
                if (foodItem != null && GetComponent<Holding>().HoldingItem == null && foodItem.GetComponent<PickUpV2>().FoodIdx != -2)
                {
                    other.GetComponent<Holding>().RemoveHolding();
                    foodItem.GetComponent<PickUpV2>().Pick(gameObject);
                    foodItem.GetComponent<PickUpV2>().Pickable = false;
                    stateManager.foodIdx = foodItem.GetComponent<PickUpV2>().FoodIdx;
                }
            }
        }
    }
}