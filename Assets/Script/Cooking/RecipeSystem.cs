using UnityEngine;
using System.Collections.Generic;

public class RecipeSystem : MonoBehaviour
{
    public static RecipeSystem Instance { get; private set; }
    private Dictionary<int, int> recipes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // only allow one instance
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        recipes = new Dictionary<int, int>();

        // Register recipes
        recipes[(int)(Ingredients.dish81 | Ingredients.dish83 | Ingredients.dish84)] = 0;
        recipes[(int)(Ingredients.dish87 | Ingredients.dish89 | Ingredients.dish91)] = 1;
        recipes[(int)(Ingredients.dish94 | Ingredients.dish96 | Ingredients.dish97)] = 2;

    }

    public int Match(int mask)
    {
        Menu menu = Menu.Instance;
        for (int i = 0; i < menu.FoodPrefabs.Length; i++)
        {
            if (recipes.TryGetValue(mask, out int result) && result == i)
                return i;
        }
        return -1; // no match found
    }

}
