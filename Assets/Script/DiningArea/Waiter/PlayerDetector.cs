using System.Collections.Generic;
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

        // private void OnTriggerStay2D(Collider2D other)
        // {
        //     if (stateManager.CurrentState is WaiterIdleState && other.CompareTag(PLAYER_TAG))
        //     {
        //         List<GameObject> foodItems = other.GetComponent<Holding>().HoldingItem;
        //         if (foodItems != null && GetComponent<Holding>().HasSpace())
        //         {
        //             foreach (var foodItem in foodItems.ToArray())
        //             {
        //                 if (foodItem.GetComponent<PickUpV2>().FoodIdx != -2)
        //                 {
        //                     other.GetComponent<Holding>().RemoveHoldingItem(foodItem);
        //                     foodItem.GetComponent<PickUpV2>().Pickable = false;
        //                     GetComponent<Holding>().PickUpItem(foodItem);
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}