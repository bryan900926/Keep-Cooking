using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplayer : MonoBehaviour
{

    public static MenuDisplayer Instance { get; private set; }
    [SerializeField] private GameObject[] menuSlotPrefab;

    [SerializeField] private IngredientData[] ingredientDatas;
    private Dictionary<int, IngredientData> ingredientDataDict;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        for (int i = 0; i < menuSlotPrefab.Length; i++)
        {
            menuSlotPrefab[i].GetComponent<MenuSlot>().Image.enabled = false;
        }

        Instance = this;

    }
    void Start()
    {
        ingredientDataDict = new Dictionary<int, IngredientData>();
        foreach (var data in ingredientDatas)
        {
            ingredientDataDict[data.Mask] = data;
        }
    }


    public void RefreshMenuSlots(List<Ingredients> ingredientsList)
    {
        Debug.Log("Refreshing Menu Slots...");
        int count = ingredientsList.Count;

        for (int i = 0; i < count; i++)
        {
            Ingredients ingredient = ingredientsList[i];
            Image image = menuSlotPrefab[i].GetComponent<MenuSlot>().Image;

            if (ingredient == Ingredients.None)
            {
                image.enabled = false;
                Debug.Log("No ingredient assigned for this slot.");
                continue;
            }

            // 🔒 Safe dictionary lookup
            if (ingredientDataDict.TryGetValue((int)ingredient, out IngredientData data))
            {
                image.sprite = data.image;
                image.enabled = true;
            }
            else
            {
                Debug.LogWarning($"No IngredientData found for ingredient index {(int)ingredient} ({ingredient})");

            }
        }
    }

}
