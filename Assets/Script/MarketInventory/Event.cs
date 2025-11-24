using UnityEngine;

[CreateAssetMenu(fileName = "New Market Event", menuName = "Market/Market Event")]
public class MarketEvent : ScriptableObject
{
    [Header("Basic Info")]
    public string eventName;       
    [TextArea(2, 5)]
    public string description;

    [Header("Event Effects")]
    public string[] goods;
    public int[] limits;
    public float[] prices;
}
