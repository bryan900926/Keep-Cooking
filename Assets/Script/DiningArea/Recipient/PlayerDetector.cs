using UnityEngine;

namespace DiningArea.Recipient
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private CanvasGroup levelUpPanel;

        [SerializeField] private GameObject hint;

        private bool isOpened = false;
        private bool playerInside = false;
        private void Start()
        {
            hint.SetActive(false);
            Toggle.Instance.ClosePanel(KeysForUI.ControlLevel);
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (isOpened)
                {
                    Toggle.Instance.ClosePanel(KeysForUI.ControlLevel);
                    isOpened = false;
                }
                else if (playerInside && !isOpened)
                {
                    Toggle.Instance.OpenPanel(KeysForUI.ControlLevel);
                    isOpened = true;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = true;
                hint.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = false;
                hint.SetActive(false);
            }
        }
    }
}

