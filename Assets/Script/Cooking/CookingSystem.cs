using UnityEngine;

public class CookingSystem : MonoBehaviour
{
    private HoldingSystem holdingSystem;
    private RecipeSystem recipeSystem;

    public GameObject counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holdingSystem = GameObject.FindGameObjectWithTag("HoldingSystem").GetComponent<HoldingSystem>();
        recipeSystem = RecipeSystem.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        int mask = 0;
        // masks of each fooditem are at RecipeSystem.cs
        foreach (GameObject food in holdingSystem.slots)
        {
            IngredientData ingredient = food.GetComponent<HoldingSlot>().currentItem;
            if (ingredient != null)
            {
                mask |= ingredient.Mask;
            }
        }

        int res = recipeSystem.Match(mask);

        if (res != -1)
        {
            Menu menu = Menu.Instance;
            foreach (GameObject food in holdingSystem.slots)
            {
                HoldingSlot slot = food.GetComponent<HoldingSlot>();
                slot.removeItem();
            }
            menu.SpawnForPlayer(res, counter.transform.position);
        }
    }
}
