using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUIFactory : MonoBehaviour
{
    public static IngredientUIFactory Instance;
    [SerializeField] private IngredientData[] ingredientDataArray;
    private Dictionary<int, IngredientData> ingredientUIPrefab = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        foreach (var data in ingredientDataArray)
        {
            ingredientUIPrefab[data.Mask] = data;
        }
    }

    public GameObject CreateIngredientUI(int ingredientId)
    {
        if (ingredientUIPrefab.ContainsKey(ingredientId))
        {
            GameObject ingredientUI = new GameObject("Placeholder");
            FoodProperty foodProperty = ingredientUI.AddComponent<FoodProperty>();
            foodProperty.Ingredient = ingredientUIPrefab[ingredientId];
            RectTransform rt = ingredientUI.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(90, 90);
            ingredientUI.AddComponent<CanvasGroup>();
            Image img = ingredientUI.AddComponent<Image>();
            img.sprite = ingredientUIPrefab[ingredientId].image;
            return ingredientUI;
        }
        Debug.LogError("Ingredient ID not found: " + ingredientId);
        return null;
    }
}