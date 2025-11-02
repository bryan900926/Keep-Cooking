using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class ClickableChef : MonoBehaviour, IPointerClickHandler
{
    private ChefStateManager chefStateManager;

    [SerializeField] private Level levelSystem;

    private void Start()
    {
        // Find parent manager on start
        chefStateManager = GetComponentInParent<ChefStateManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Chef clicked");
        if (chefStateManager != null)
        {
            levelSystem.LevelUp();
            // Craftingv2.Instance.SetCurrentChef(chefStateManager.gameObject);
            // Toggle.Instance.OpenPanel(Key.V);
        }
    }
}
