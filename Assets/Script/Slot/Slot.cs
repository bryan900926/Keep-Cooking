using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    protected Image image;
    public IngredientData currentItem;

    protected virtual void Update()
    {
        if (currentItem == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = currentItem.image;
        }
    }
}
