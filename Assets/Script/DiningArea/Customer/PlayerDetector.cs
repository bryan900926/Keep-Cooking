using UnityEngine;
using UnityEngine.InputSystem;

namespace DiningArea.Customer
{
    public class PlayerDetector : MonoBehaviour
    {

        private const string PLAYER_TAG = "Player";

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG) && GetComponent<CustomerStateManager>().CurrentState is CustomerWaitFoodState && Keyboard.current.rKey.isPressed)
            {
                BackControl.Instance.RecruitChef(gameObject);
                Debug.Log("Customer became Chef");
            }

        }
    }
}

