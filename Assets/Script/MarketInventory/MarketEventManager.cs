using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketEventManager : MonoBehaviour
{
    public static MarketEventManager Instance;

    public MarketEvent currentEvent;

    public Image background;


    public List<MarketEvent> allEvents = new List<MarketEvent>();

    public GameObject[] Eventsprefab;

    private BlackOverlayController blackOverlayController;

    private void Awake()
    {
        Instance = this;
        blackOverlayController = GetComponent<BlackOverlayController>();    
    }

    public void RecoverEvent(MarketEvent e)
    {
        MarketInventory.Instance.RecoverLimit(10);

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
        if (allEvents.Count == 0) return;
        int randomIndex = Random.Range(0, allEvents.Count);
        MarketEvent randomEvent = allEvents[randomIndex];
        ApplyMarketEvent(randomEvent);
        currentEvent = randomEvent;
        allEvents.RemoveAt(randomIndex);
    }

    private void ApplyMarketEvent(MarketEvent e)
    {
        if (currentEvent != null)
            RecoverEvent(currentEvent);
        CenterMessage.Instance.ShowMessage(e.description);
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

        switch (e.eventName)
        {
                case "Ghostly Thief":
                GameObject ghost = Instantiate(Eventsprefab[0], transform.position, Quaternion.identity);
                if (ghost!= null)
                {
                    ghost.GetComponent<Ghost>().Appear();   
                }
                MarketInventory.Instance.Disappear();
                MarketInventory.Instance.UpdateMenu();
                break;

                 
        }
    }
}
