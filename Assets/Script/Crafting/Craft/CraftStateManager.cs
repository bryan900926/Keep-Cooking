using System.Collections.Generic;
using UnityEngine;

public class CraftStateManager : MonoBehaviour
{
    public static CraftStateManager Instance;
    private CraftState currentState;

    [SerializeField] private GameObject[] slots = new GameObject[9];

    public List<Ingredients> multipleIngredients = new List<Ingredients>(9);

    public enum Recipe_status
    {
        Normal,
        Random,
        Mission
    }

    private Recipe_status status = Recipe_status.Normal;
    public Recipe_status Status { get => status; set => status = value; }

    private GameObject currentChef;

    public GameObject CurrentChef { get => currentChef; set => currentChef = value; }

    private bool isUsing = false;

    public bool IsUsing { get => isUsing; set => isUsing = value; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        ChangeState(new CraftIdleState(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(CraftState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void MatchbyStatus(List<Ingredients> recipe)
    {
        if (status == Recipe_status.Normal)
            Match(recipe, Recipe.instance.Normal_recipe);
        else if (status == Recipe_status.Random)
            Match(recipe, Recipe.instance.Random_recipe);
        else Match(recipe, Recipe.instance.Mission_recipe);
    }

    public void Match(List<Ingredients> inputRecipe, Dictionary<List<Ingredients>, GameObject> reference)
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
                Toggle.Instance.ClosePanel(Toggle.keyOpenCrafting);
                CenterMessage.Instance.ShowMessage(CenterMessage.SUCCESSFUL_COOK);
                isUsing = false;
                chefStateManager?.EnableCooking(foodidx);
                return;
            }
        }
        if (currentChef != null)
            chefStateManager?.EnableCooking(-2);
        isUsing = false;
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

    }

}