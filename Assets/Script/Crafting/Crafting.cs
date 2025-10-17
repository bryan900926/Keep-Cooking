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

    private GameObject currentChef;

    public GameObject CurrentCheft { get => currentChef; set => currentChef = value; }

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
        if (currentChef == null) return;
        ChefStateManager chefStateManager = currentChef.GetComponent<ChefStateManager>();
        foreach (var pair in reference)
        {
            List<Ingredients> correctRecipe = pair.Key;
            GameObject foodPrefab = pair.Value;

            if (currentChef != null && AreListsEqualInOrder(correctRecipe, inputRecipe))
            {
                int foodidx = foodPrefab.GetComponent<DishProperty>().Foodidx;
                if (chefStateManager == null)
                {
                    Debug.LogWarning("Chef leaved");
                    Toggle.Instance.CloseAllUIPanels();
                    return;
                }
                chefStateManager?.EnableCooking(foodidx);
                return;
            }
        }
        chefStateManager?.EnableCooking(-2);
        Toggle.Instance.CloseAllUIPanels();
        Debug.Log("not match any recipe");
    }

    private bool AreListsEqualInOrder(List<Ingredients> a, List<Ingredients> b)
    {
        if (a == null || b == null || a.Count != b.Count) return false;

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
