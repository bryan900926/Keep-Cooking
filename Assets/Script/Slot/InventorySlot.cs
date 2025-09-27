using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    GameObject holdingSystem;
    [SerializeField] private Button button;
    void Start()
    {
        image = GetComponentInChildren<Image>();
        holdingSystem = GameObject.FindWithTag("HoldingSystem");
        if (button != null)
            button.onClick.AddListener(OnClick);
    }
    protected override void Update()
    {
        base.Update();

    }

    public void OnClick()
    {
        if (currentItem != null)
        {
            holdingSystem.GetComponent<HoldingSystem>().AddIngredient(currentItem);
        }
    }

}
