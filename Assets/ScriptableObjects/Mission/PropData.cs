
using UnityEngine;

[CreateAssetMenu(fileName = "NewPropData", menuName = "Mission/PropData")]
public class PropData : ScriptableObject
{
    public enum Tools
    {
        SCROLL,
        WATER,
        DRINK,
        BEER,
    } // Enum dropdown
    public Tools Type;
    public Sprite Image;
}