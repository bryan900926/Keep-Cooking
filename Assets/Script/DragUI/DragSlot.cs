using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IDropHandler
{
    private Ingredients Ingredients;
    [SerializeField] int slotindex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drooooooooooooooop");
        var droppedItem = eventData.pointerDrag.GetComponent<RectTransform>();
        droppedItem.SetParent(transform);
        droppedItem.anchoredPosition = Vector2.zero;

        var dragScript = eventData.pointerDrag.GetComponent<DragInterface>();
        if (dragScript != null)
        {
            dragScript.OnDroppedSuccessfully(slotindex);
        }

        Ingredients = eventData.pointerDrag.GetComponent<FoodProperty>().Ingredient.type;

        Crafting.instance.SetIngredient(slotindex, Ingredients);

        Crafting.instance.DebugItem();
    }

}

