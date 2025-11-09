using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChefRecipe : MonoBehaviour
{
    // 🔹 Notify others when a recipe changes
    public event Action<bool> OnRecipeChanged;

    private readonly Dictionary<int, List<Ingredients>> currentRecipes = new();

    private bool[] correctRecipeStats;
    void Start()
    {
        correctRecipeStats = new bool[Menu.Instance.FoodPrefabs.Length - 1];  // exclude last "beer" dish
        for (int i = 0; i < Menu.Instance.FoodPrefabs.Length - 1; i++)
        {
            currentRecipes[i] = Recipe.instance.Food_recipes[i];
            correctRecipeStats[i] = true;

        }
        OnRecipeChanged?.Invoke(RecipeIsCorrect());
    }

    public void UpdateRecipe(int foodIdx, List<Ingredients> updatedRecipe)
    {
        List<Ingredients> recipeCopy = new(updatedRecipe);

        // store or replace
        if (currentRecipes.ContainsKey(foodIdx))
            currentRecipes[foodIdx] = recipeCopy;
        else
            currentRecipes.Add(foodIdx, recipeCopy);

        bool isCorrect = Recipe.instance.CheckRecipeCorrect(foodIdx, recipeCopy);
        correctRecipeStats[foodIdx] = isCorrect;
        OnRecipeChanged?.Invoke(RecipeIsCorrect());

    }

    public List<Ingredients> GetRecipe(int foodIdx)
    {
        if (currentRecipes.ContainsKey(foodIdx))
            return currentRecipes[foodIdx];

        return new List<Ingredients>(Enumerable.Repeat(Ingredients.None, 9));
    }

    private bool RecipeIsCorrect()
    {
        foreach (var status in correctRecipeStats)
        {
            if (!status) return false;
        }
        return true;
    }

    public void RandomizeRecipe(int foodIdx)
    {
        if (currentRecipes.ContainsKey(foodIdx))
        {
            List<Ingredients> randomizedRecipe = Recipe.instance.RandomRecipe(currentRecipes[foodIdx]);
            UpdateRecipe(foodIdx, randomizedRecipe);
        }
        else
        {
            Debug.LogWarning($"No existing recipe found for foodIdx {foodIdx} to randomize.");
        }
    }
}
