using System.Collections.Generic;
using UnityEngine;

public class Craftingv2 : MonoBehaviour
{
    public static Craftingv2 Instance;

    [SerializeField] private GameObject[] slots = new GameObject[9];
    public List<Ingredients> multipleIngredients;
    public Vector2 abc;
    public enum Recipe_status
    {
        Normal,
        Random,
        Mission
    }

    private Recipe_status status = Recipe_status.Normal;
    public Recipe_status Status { get => status; set => status = value; }

    private GameObject currentChef;

    private int currentDishIdx = -1;

    public int CurrentDishIdx { get => currentDishIdx; set => currentDishIdx = value; }


    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
        multipleIngredients = new List<Ingredients>(9);
        for (int i = 0; i < 9; i++)
        {
            multipleIngredients.Add(Ingredients.None);
        }
    }
    public void SetIngredient(int slotIndex, Ingredients Data)
    {
        multipleIngredients[slotIndex] = Data;
    }

    public void DeleteIngredient(int slotIndex)
    {
        multipleIngredients[slotIndex] = 0;
    }

    public void CorrectRecipe()
    {
        if (currentDishIdx == -1)
        {
            Debug.LogWarning("Invalid dish index.");
            return;
        }
        Debug.Log("Correcting recipe for dish index: " + currentDishIdx);
        if (currentChef == null)
        {
            CenterMessage.Instance.ShowMessage(CenterMessage.NO_CHEF);
            return;
        }
        currentChef.GetComponent<ChefRecipe>().UpdateRecipe(currentDishIdx, multipleIngredients);
    }

    public void Match(ChefStateManager chefStateManager, List<Ingredients> inputRecipe, Dictionary<List<Ingredients>, GameObject> reference)
    {
        bool findMatch = false;
        foreach (var pair in reference)
        {
            List<Ingredients> correctRecipe = pair.Key;
            GameObject foodPrefab = pair.Value;

            if (chefStateManager != null && AreListsEqualInOrder(inputRecipe, correctRecipe))
            {
                int foodidx = foodPrefab.GetComponent<DishProperty>().Foodidx;
                findMatch = true;
                break;
            }
        }
        if (!findMatch && chefStateManager != null)
        {
            CenterMessage.Instance.ShowMessage(CenterMessage.FAILED_COOK);
        }
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
    }

    public void SetCurrentChef(GameObject chef)
    {
        if (chef != null && chef == currentChef) return;
        ChefStateManager chefManager = chef.GetComponent<ChefStateManager>();

        if (currentChef != null)
        {
            ChefStateManager currentChefManager = currentChef.GetComponent<ChefStateManager>();
            if (currentChefManager != null)
                currentChefManager.OnChefDestroyed -= OnChefDestroyed;
        }

        currentChef = chef;
        currentDishIdx = 0;
        GetRecipeFromChef();

        if (chef != null)
        {
            chefManager = chef.GetComponent<ChefStateManager>();
            if (chefManager != null)
                chefManager.OnChefDestroyed += OnChefDestroyed;
        }
    }

    private void OnChefDestroyed()
    {
        currentChef = null;
        CenterMessage.Instance.ShowMessage(CenterMessage.CHEF_LEAVE);
        Toggle.Instance.ClosePanel(Toggle.keyOpenCrafting);
    }
    public void GetRecipeFromChef()
    {
        if (currentChef == null || currentDishIdx < 0) return;
        ChefRecipe chefRecipe = currentChef.GetComponent<ChefRecipe>();
        List<Ingredients> recipe = chefRecipe.GetRecipe(currentDishIdx);
        if (recipe.Count != multipleIngredients.Count) return;
        ClearALL();
        for (int i = 0; i < recipe.Count; i++)
        {
            multipleIngredients[i] = recipe[i];
            if (recipe[i] == Ingredients.None) continue;
            var ingredientUI = IngredientUIFactory.Instance.CreateIngredientUI((int)recipe[i]);
            ingredientUI.transform.SetParent(slots[i].transform, false);
        }

    }
}
