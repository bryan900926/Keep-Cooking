[System.Serializable]
public class MarketSlot
{
    public IngredientData item;
    public string name;
    public int amount;
    public int price;
    public int limited;
    public int Currentcount;

    public MarketSlot(IngredientData item)
    {
        this.item = item;
        this.name = item.name;
        this.amount = 0;
        this.price = 0;
        this.limited = 0;
        this.Currentcount = 0;
    }
}
