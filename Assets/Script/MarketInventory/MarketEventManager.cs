using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketEventManager : MonoBehaviour
{
    public static MarketEventManager Instance;

    public MarketEvent currentEvent = null;

    public Image background;


    public List<MarketEvent> allEvents = new List<MarketEvent>();

    public GameObject[] Eventsprefab;

    private BlackOverlayController blackOverlayController;

    private float eventmultiplier;

    private string propname;

    private bool First = true;

    [SerializeField] private int interval = 0;

    [SerializeField] private int totaltime = 0;

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
        First = false;
        allEvents.RemoveAt(randomIndex);
    }

    private void ApplyMarketEvent(MarketEvent e)
    {
        if (currentEvent != null && First == false)
            RecoverEvent(currentEvent);
            CustomerPropertyManager.Instance.Updateeveryone(eventmultiplier, propname, false);
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

                case "Time-Loop Hour":
                StartCoroutine(PlayTimedropMultiple(5, 30));
                //eventmultiplier = 0.5f;
                //propname = "MovingSpeed";
                //CustomerPropertyManager.Instance.Updateeveryone(eventmultiplier,propname, true);
                break;

                case "Sandworm Invasion":
                var holemanager = GetComponent<Holemanager>();
                holemanager.SandwormEvent(2f, 50f);
                break;

                case "Unstable Portal":
                GameObject crack = Instantiate(Eventsprefab[0], new Vector3(2f, 2f, 0f), Quaternion.identity); // location needs to adjust
                if (crack != null)
                {
                    crack.GetComponent<Crack>().ExpandCrack(3f, 15f);
                }
                break;

                case "Large Coin":
                StartCoroutine(LargeCoin(20));
                break;
                



        }
    }

    private IEnumerator PlayTimedropMultiple(int interval , int totalDuration)
    {

        int count = totalDuration / interval;
        for (int i = 0; i < count; i++)
        {
            // 生成 timedrop
            GameObject timedrop = Instantiate(
                Eventsprefab[0],
                new Vector3(0f, 10f, 0f),
                Quaternion.identity
            );

            if (timedrop != null)
            {
                Timedrop drop = timedrop.GetComponent<Timedrop>();
                drop.PlayWaterDrop(drop.startposition, drop.hitposition);
            }

            // 更新菜單（如果要每次都更新）
            MarketInventory.Instance.UpdateMenu();

            // 等下一次
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator LargeCoin(int totalDuration)
    {
        var spawner = FindFirstObjectByType<CustomerSpawner>();
        Debug.Log(spawner);
        if (spawner != null)
        {
            spawner.LargeCoin = true;
        }
        yield return new WaitForSeconds(totalDuration);
        spawner.LargeCoin = false;

    }
}
