using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ClickableChef : MonoBehaviour, IPointerClickHandler
{
    private ChefStateManager chefStateManager;

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
            Craftingv2.Instance.SetCurrentChef(chefStateManager.gameObject);
            Toggle.Instance.OpenPanel(Key.V);

        }
    }
}
