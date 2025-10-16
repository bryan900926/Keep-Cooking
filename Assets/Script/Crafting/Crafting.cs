using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public static Crafting instance;

    [SerializeField] private GameObject[] slots = new GameObject[9];
    public List<Ingredients> multipleIngredients = new List<Ingredients>(9);
    public Vector2 abc;
    public enum Recipe_status
    {
        Normal,
        Random,
        Mission
    }

    private Recipe_status status = Recipe_status.Normal;

    private GameObject currentCheft;

    public GameObject CurrentCheft { get => currentCheft; set => currentCheft = value; }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetIngredient(int slotIndex, Ingredients Data)
    {
        multipleIngredients[slotIndex] = Data;
    }

    public void DeleteIngredient(int slotIndex)
    {
        multipleIngredients[slotIndex] = 0;
    }

    public void Cook()
    {
        MatchbyStatus(multipleIngredients);
    }

    public void MatchbyStatus(List<Ingredients> recipe)
    {
        if (status == Recipe_status.Normal)
            Match(recipe, Recipe.instance.Normal_recipe, abc);
        else if (status == Recipe_status.Random)
            Match(recipe, Recipe.instance.Random_recipe, abc);
        else Match(recipe, Recipe.instance.Mission_recipe, abc);
    }

    public void Match(List<Ingredients> inputRecipe, Dictionary<List<Ingredients>, GameObject> reference, Vector2 spawnPos)
    {
        for (int i = 0; i < inputRecipe.Count; i++)
        {
            Debug.Log($"slot idx: {i} + {inputRecipe[i]}");
        }
        foreach (var pair in reference)
        {
            List<Ingredients> correctRecipe = pair.Key;
            GameObject foodPrefab = pair.Value;

            if (AreListsEqualInOrder(correctRecipe, inputRecipe))
            {
                if (currentCheft == null)
                {
                    Debug.LogError("no chef available");
                    return;
                }
                ChefStateManager chefStateManager = currentCheft.GetComponent<ChefStateManager>();
                int foodidx = foodPrefab.GetComponent<DishProperty>().Foodidx;
                if (foodidx < 0)
                {
                    Debug.LogError("food idx error");
                    return;
                }
                chefStateManager.EnableCooking(foodidx);
                return;
            }
        }

        Debug.Log("not match any recipe");
    }

    private bool AreListsEqualInOrder(List<Ingredients> a, List<Ingredients> b)
    {
        if (a == null || b == null) return false;
        if (a.Count != b.Count) return false;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    public void ClearALL()
    {
        for (int i = 0; i < multipleIngredients.Count; i++)
        {
            multipleIngredients[i] = 0;
            foreach (Transform child in slots[i].transform)
            {
                Destroy(child.gameObject);
            }
        }

        DebugItem();
    }

    public void DebugItem()
    {
        for (int i = 0; i < multipleIngredients.Count; i++)
        {
            Debug.Log(multipleIngredients[i]);
        }
    }
}
