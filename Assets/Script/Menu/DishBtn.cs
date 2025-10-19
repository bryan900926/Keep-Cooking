using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishBtn : MonoBehaviour
{
    private Button button;
    [SerializeField] private int dishID;

    void Start()
    {
        button = GetComponent<Button>();
        UpdateButtonAction();
    }

    // 🔁 If your dishID changes at runtime, you can call this again
    public void UpdateButtonAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnDishClicked);
    }

    private void OnDishClicked()
    {
        // This is the method actually called by the button click
        DishProperty dishProperty = Recipe.instance.Dishes[dishID].GetComponent<DishProperty>();
        List<Ingredients> ingredients = dishProperty.GetCurrentRecipe();

        MenuDisplayer.Instance.RefreshMenuSlots(ingredients);

        // You can now do whatever you need with the recipe
        Debug.Log($"Dish {dishID} clicked, recipe has {ingredients.Count} ingredients!");
        // Example: display it in UI, etc.
    }
}
