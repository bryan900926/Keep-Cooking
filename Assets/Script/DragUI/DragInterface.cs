using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragInterface : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform RectTrans;
    private CanvasGroup CanvasGroup;
    private FoodProperty FoodProperty;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private bool dropped = true;
    private bool droppeddrag = false;
    private int slotindex;
    private GameObject placeholder; // occupied object
    private Transform draglayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        RectTrans = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
        FoodProperty = GetComponent<FoodProperty>();

        originalParent = transform.parent;
        originalAnchoredPosition = RectTrans.anchoredPosition;
        draglayer = GameObject.Find("Draglayer").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (dropped && !droppeddrag)
        {
            CreateObject();
        }

        if (droppeddrag)
        {
            Crafting.instance.DeleteIngredient(slotindex);
        }

        dropped = false;

        transform.SetParent(draglayer); // Move to the independent DragLayer

        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0.65f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dropped)
        {
            transform.SetParent(originalParent);
            RectTrans.anchoredPosition = originalAnchoredPosition;
            if (gameObject.name == "Placeholder")
                Destroy(gameObject);
        }

        if (placeholder != null)
        {
            //Destroy(placeholder);
        }


        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTrans.anchoredPosition += eventData.delta; // Plus the additional distance
    }

    public void OnDroppedSuccessfully(int index)
    {
        dropped = true;
        droppeddrag = true;
        slotindex = index;

        Debug.Log($"{gameObject.name} dropped successfully!");
    }

    public void CreateObject()
    {
        placeholder = new GameObject("Placeholder");

        // About Gameobject position
        placeholder.transform.SetParent(originalParent, false);
        placeholder.transform.localPosition = RectTrans.localPosition;
        placeholder.transform.localRotation = RectTrans.localRotation;
        placeholder.transform.localScale = RectTrans.localScale; // adjust the size
        placeholder.AddComponent<DragInterface>();

        placeholder.AddComponent<FoodProperty>();
        var food = placeholder.GetComponent<FoodProperty>();
        food.Ingredient = FoodProperty.Ingredient;



        placeholder.AddComponent<RectTransform>();
        var phRect = placeholder.GetComponent<RectTransform>();
        phRect.sizeDelta = new Vector2(90, 90);

        placeholder.AddComponent<CanvasGroup>();
        var cg = placeholder.GetComponent<CanvasGroup>();

        var img = placeholder.AddComponent<Image>();
        var originalImage = GetComponent<Image>();
        if (originalImage != null)
        {
            img.sprite = originalImage.sprite;
            img.color = new Color(1, 1, 1, 1f);
            img.raycastTarget = true; // �קK�v�T�ƥ�
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
