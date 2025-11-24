using UnityEngine;

public class MarketEventManager : MonoBehaviour
{
    public static MarketEventManager Instance;

    public MarketEvent currentEvent;
    
    public MarketEvent[] allEvents;

    private void Awake()
    {
        Instance = this;
    }

    public void RecoverEvent(MarketEvent e)
    {
        MarketInventory.Instance.RecoverLimit(20);

        string[] goods = e.goods;
        float[] prices = e.prices;

        for (int i = 0; i < goods.Length; i++)
        {
            string itemName = goods[i];

            if (i < prices.Length)
            {
                float priceMultiplier = prices[i];
                MarketInventory.Instance.ChangePrice(itemName, priceMultiplier, true);
            }
        }
    }

    public void TriggerRandomEvent()
    {
        if (allEvents.Length == 0) return;
        int randomIndex = Random.Range(0, allEvents.Length);
        MarketEvent randomEvent = allEvents[randomIndex];
        ApplyMarketEvent(randomEvent);
        currentEvent = randomEvent;
    }

    private void ApplyMarketEvent(MarketEvent e)
    {
        if (currentEvent != null)
            RecoverEvent(currentEvent);

        MarketInventory.Instance.UpdateMenu();

        string[] goods = e.goods;
        int[] limits = e.limits;
        float[] prices = e.prices;

        for (int i = 0; i < goods.Length; i++)
        {
            string itemName = goods[i];

            if (i < limits.Length)
            {
                int limitChange = limits[i];
                MarketInventory.Instance.ChangeLimit(itemName, limitChange);
            }

            if (i < prices.Length)
            {
                float priceMultiplier = prices[i];
                MarketInventory.Instance.ChangePrice(itemName, priceMultiplier, false);
            }
        }

        for (int i = 1; i < 4; i++)
        {
            MarketUI.Instance.RefreshUI(i);
        }
        
        Debug.Log("Triggered Market Event: " + e.eventName);
    }
}
