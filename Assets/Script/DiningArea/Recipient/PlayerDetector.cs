using UnityEngine;

namespace DiningArea.Recipient
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private CanvasGroup levelUpPanel;
        private void Start()
        {
            ToggleLevelUpPanel(false);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Toggle.Instance.ToggleUIRoot(true);
                ToggleLevelUpPanel(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Toggle.Instance.ToggleUIRoot(false);
                ToggleLevelUpPanel(false);
            }
        }

        private void ToggleLevelUpPanel(bool isVisible)
        {
            if (levelUpPanel == null) return;
            levelUpPanel.alpha = isVisible ? 1f : 0f;
            levelUpPanel.interactable = isVisible;
            levelUpPanel.blocksRaycasts = isVisible;
        }
    }
}

