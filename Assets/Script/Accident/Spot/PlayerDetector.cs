using UnityEngine;
using UnityEngine.InputSystem;

namespace Accident.Spot
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private GameObject hint;

        [SerializeField] private OilSpotSpawner oilSpotSpawner;
        private bool playerInside = false; // track if player is in trigger

        private SpriteRenderer hintRenderer;

        void Start()
        {
            hintRenderer = hint.GetComponent<SpriteRenderer>();
            hintRenderer.enabled = false;
        }

        void Update()
        {
            if (playerInside && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name + " entered oil spot");
            playerInside = true;
            ShowHint(true);

            if (other.TryGetComponent<Holding>(out Holding holding))
            {
                holding.RemoveAllHolding();
            }
            if (other.TryGetComponent<WaiterStateManager>(out WaiterStateManager waiterStateManager))
            {
                waiterStateManager.ChangeState(new WaiterIdleState(waiterStateManager));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            playerInside = false;
            ShowHint(false);
        }

        private void ShowHint(bool show)
        {
            hintRenderer.enabled = show;
        }
    }
}
