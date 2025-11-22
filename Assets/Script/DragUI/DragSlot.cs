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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        droppedItem.SetParent(transform);
        droppedItem.anchoredPosition = Vector2.zero;

        if (eventData.pointerDrag.TryGetComponent<DragInterface>(out var dragScript))
        {
            dragScript.OnDroppedSuccessfully(slotindex);
        }

        Ingredients = eventData.pointerDrag.GetComponent<FoodProperty>().Ingredient.type;

        Craftingv2.Instance.SetIngredient(slotindex, Ingredients);

    }

    public void InitData()
    {

    }

}

