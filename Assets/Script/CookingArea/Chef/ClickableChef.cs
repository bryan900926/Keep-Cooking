using UnityEngine.EventSystems;

public class ClickableChef : ClickableCharacter<ChefStateManager>
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Craftingv2.Instance.SetCurrentChef(stateManager.gameObject);
        Toggle.Instance.OpenPanel(KeysForUI.Crafting);
    }
}
