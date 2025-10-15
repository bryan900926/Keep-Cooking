using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "NewDish", menuName = "Cooking/Dish")]
public class DishData : ScriptableObject
{
    public List<Ingredients> Normal_recipe;
    public List<Ingredients> Random_recipe;
    public List<Ingredients> Mission_recipe;   // Enum dropdown
    public Sprite image;

}
