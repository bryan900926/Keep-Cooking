using UnityEngine;
using System.Collections.Generic;

public class DishProperty : MonoBehaviour
{
    public List<Ingredients> normal_recipe = new List<Ingredients>();
    public List<Ingredients> random_recipe = new List<Ingredients>();
    public List<Ingredients> mission_recipe = new List<Ingredients>();

    [SerializeField] private int foodidx;

    public int Foodidx { get => foodidx; set => foodidx = value; }

    public enum DishType
    {
        Normal,
        Random,
        Mission
    }
    [System.NonSerialized]
    private DishType state = DishType.Normal;
    public DishType State { get => state; set => state = value; }

    void Update()
    {

    }
    public List<Ingredients> GetCurrentRecipe()
    {
        Debug.Log($"Getting recipe for dish {foodidx} of type {state}");
        switch (state)
        {
            case DishType.Normal:
                return normal_recipe;
            case DishType.Random:
                return random_recipe;
            case DishType.Mission:
                return mission_recipe;
            default:
                return normal_recipe;
        }
    }

}
