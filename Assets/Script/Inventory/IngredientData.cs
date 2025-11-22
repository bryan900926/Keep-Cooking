using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewIngredient", menuName = "Cooking/Ingredient")]
public class IngredientData : ScriptableObject
{
    public Ingredients type;   // Enum dropdown
    public string Name;
    public Sprite image;

    public int Mask => (int)type;
}