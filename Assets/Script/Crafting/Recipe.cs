using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Recipe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Recipe instance;
    [SerializeField] private GameObject[] dishes;

    public Dictionary<List<Ingredients>, GameObject> Normal_recipe = new Dictionary<List<Ingredients>, GameObject>();
    public Dictionary<List<Ingredients>, GameObject> Random_recipe = new Dictionary<List<Ingredients>, GameObject>();
    public Dictionary<List<Ingredients>, GameObject> Mission_recipe = new Dictionary<List<Ingredients>, GameObject>();

    public GameObject[] Dishes
    {
        get => dishes;
        set => dishes = value;
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        BuildRecipeDictionary();
    }

    public List<Ingredients> RandomRecipe(List<Ingredients> recipe)
    {
        List<Ingredients> newRecipe = recipe.ToList();

        int n = newRecipe.Count;
        for (int i = n - 1; i > 0; i--)
        {
            if (newRecipe[i] == Ingredients.None)
                continue;
            int j = UnityEngine.Random.Range(0, n);
            Ingredients temp = newRecipe[i];
            newRecipe[i] = newRecipe[j];
            newRecipe[j] = temp;
        }

        return newRecipe;
    }

    private void BuildRecipeDictionary()
    {
        Normal_recipe.Clear();

        foreach (var dish in Dishes)
        {
            var property = dish.GetComponent<DishProperty>();
            if (property == null)
            {
                Debug.LogWarning($"{dish.name} skip!!!");
                continue;
            }
            List<Ingredients> recipe = property.normal_recipe;
            if (recipe == null || recipe.Count != 9)
            {
                Debug.LogWarning($"{dish.name} recipe is empty.");
                continue;
            }

            List<Ingredients> key = new List<Ingredients>(recipe);

            if (!Normal_recipe.ContainsKey(key))
                Normal_recipe.Add(key, dish);
            else
                Debug.LogWarning($"{dish.name}");
        }

        Debug.Log($"{Normal_recipe.Count} are built");
    }

    public void BuildOtherRecipeDictionary()
    {
        foreach (var dish in Dishes)
        {
            var property = dish.GetComponent<DishProperty>();
            property.random_recipe = RandomRecipe(property.normal_recipe);
            Random_recipe.Add(property.random_recipe, dish);
        }
        Crafting.instance.Status = Crafting.Recipe_status.Random;
        foreach (var dish in Dishes)
        {
            var property = dish.GetComponent<DishProperty>();
            property.State = DishProperty.DishType.Random;
        }

    }

}
