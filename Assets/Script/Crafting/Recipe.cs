using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Recipe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Recipe instance;
    public List<GameObject> Dishes = new List<GameObject>();
    
    public Dictionary<List<Ingredients>, GameObject> Normal_recipe = new Dictionary<List<Ingredients>, GameObject>();
    public Dictionary<List<Ingredients>, GameObject> Random_recipe = new Dictionary<List<Ingredients>, GameObject>();
    public Dictionary<List<Ingredients>, GameObject> Mission_recipe = new Dictionary<List<Ingredients>, GameObject>();


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

    // Update is called once per frame
    void Update()
    {
        //if (Event is occurred) renew the random dictionary
    }



    public List<Ingredients> RandomRecipe(List<Ingredients> recipe)
    {
        List<Ingredients> newRecipe = recipe.ToList();

        int n = newRecipe.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Ingredients temp = newRecipe[i];
            newRecipe[i] = newRecipe[j];
            newRecipe[j] = temp;
        }

        return newRecipe;
    }

    void BuildRecipeDictionary()
    {
        Normal_recipe.Clear();

        foreach (var dish in Dishes)
        {
            if (dish == null) continue;

            var property = dish.GetComponent<DishProperty>();
            if (property == null)
            {
                Debug.LogWarning($"{dish.name} skip!!!");
                continue;
            }

            List<Ingredients> recipe = property.normal_recipe;
            if (recipe == null || recipe.Count == 0)
            {
                Debug.LogWarning($"{dish.name} is empty.");
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

}
