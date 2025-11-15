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
    [SerializeField] private float freshness = 50f;

    public float Freshness { get => freshness;}

    [SerializeField] private GameObject decaySmokePrefab;
    private void Start()
    {
        if (decaySmokePrefab != null)
        {
            decaySmokePrefab.SetActive(false);
        }
    }
    void Update()
    {
        if (freshness > 0)
        {
            DecayFreshness();
        }
    }
    public List<Ingredients> GetCurrentRecipe()
    {
        Debug.Log($"Getting recipe for dish {foodidx} of type {state}");
        return state switch
        {
            DishType.Normal => normal_recipe,
            DishType.Random => random_recipe,
            DishType.Mission => mission_recipe,
            _ => normal_recipe,
        };
    }

    private void DecayFreshness()
    {
        freshness -= Time.deltaTime;
        if (freshness < 0 && decaySmokePrefab != null)
        {
            decaySmokePrefab.SetActive(true);
        }
    }

}
