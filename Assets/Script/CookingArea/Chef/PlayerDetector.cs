using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chef.Detector
{
    public class PlayerDetector : MonoBehaviour
    {
        private const String PLAYER_TAG = "Player";
        private bool playerInside = false; // track if player is in trigger
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
                int resIdx = HoldingSystem.Instance.FindProp(PropData.Tools.DRINK);
                HoldingSystem.Instance.RemoveProp(resIdx);
                if (resIdx != -1)
                {
                    GetComponent<ChefStateManager>().Energy.IsReplenishing = true;
                }
            }
        }
    }

}
