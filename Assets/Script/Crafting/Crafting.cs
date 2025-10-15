using System.Collections.Generic;
using UnityEngine;
using static Recipe;

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

    public Recipe_status status = Recipe_status.Normal;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach (var pair in reference)
        {
            List<Ingredients> correctRecipe = pair.Key;
            GameObject prefab = pair.Value;

            if (AreListsEqualInOrder(correctRecipe, inputRecipe))
            {
                Instantiate(prefab, spawnPos, Quaternion.identity);
                Debug.Log($"製作中{prefab.name}...");
                return;
            }
        }

        Debug.Log("未知食物");
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
