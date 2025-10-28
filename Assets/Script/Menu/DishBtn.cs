using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishBtn : MonoBehaviour
{
    private Button button;
    [SerializeField] private int dishID;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color darkColor = Color.gray;

    [SerializeField] private DishBtnManager dishBtnManager;

    private Image icon;

    void Start()
    {
        button = GetComponent<Button>();
        icon = GetComponent<Image>();
        UpdateButtonAction();
    }

    public void UpdateButtonAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnDishClicked);
    }

    private void OnDishClicked()
    {
        DishProperty dishProperty = Recipe.instance.Dishes[dishID].GetComponent<DishProperty>();
        List<Ingredients> ingredients = dishProperty.GetCurrentRecipe();

        MenuDisplayer.Instance.RefreshMenuSlots(ingredients);
        dishBtnManager.OnDishBtnClicked(this);

        Debug.Log($"Dish {dishID} clicked, recipe has {ingredients.Count} ingredients!");
    }
    public void SetSelected(bool isSelected)
    {
        icon.color = isSelected ? darkColor : normalColor;
    }
}
