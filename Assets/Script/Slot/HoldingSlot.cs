using UnityEngine;
using UnityEngine.UI;

public class HoldingSlot : Slot
{
    [SerializeField] private Button button;


    void Start()
    {
        image = GetComponentInChildren<Image>();
        if (button != null)
            button.onClick.AddListener(removeItem);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void removeItem()
    {
        if (currentItem != null)
        {
            currentItem = null;
        }
    }
}
