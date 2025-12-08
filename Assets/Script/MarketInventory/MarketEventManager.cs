using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketEventManager : MonoBehaviour
{
    public static MarketEventManager Instance;

    public MarketEvent currentEvent = null;

    public Image background;


    [SerializeField] private List<MarketEvent> allEvents;

    private List<MarketEvent> availableEvents;

    public GameObject[] Eventsprefab;

    private BlackOverlayController blackOverlayController;

    private float eventmultiplier;

    private string propname;

    private bool First = true;

    [SerializeField] private float interval = 0;

    [SerializeField] private float totaltime = 0;
    readonly private Dictionary<string, Action> eventActions = new();

    private void Awake()
    {
        Instance = this;
        blackOverlayController = GetComponent<BlackOverlayController>();
        availableEvents = new List<MarketEvent>(allEvents);
    }

    void Start()
    {
        eventActions.Add("Ghostly Thief", GhostThief);
        eventActions.Add("Time-Loop Hour", () => StartCoroutine(PlayTimedropMultiple(interval, totaltime)));
        eventActions.Add("Sandworm Invasion", () =>  SandwormInvasion(interval, totaltime));
        eventActions.Add("Unstable Portal", () =>  UnstablePortal(interval, totaltime));
        eventActions.Add("Large Coin", () => StartCoroutine(LargeCoin(totaltime)));
        eventActions.Add("Goblin Rampage", () => StartCoroutine(Goblin(interval, totaltime)));
        eventActions.Add("Transparent customers", () => StartCoroutine(Transparent(totaltime)));
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
        if (availableEvents.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, availableEvents.Count);
        MarketEvent randomEvent = availableEvents[randomIndex];

        ApplyMarketEvent(randomEvent);
        currentEvent = randomEvent;

        // 從可用列表移除，避免重複
        availableEvents.RemoveAt(randomIndex);
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

        if (eventActions.TryGetValue(e.eventName, out Action action))
        {
            action.Invoke();
        }
        else
        {
            Debug.LogWarning("No action defined for event: " + e.eventName);
        }
    }

    private void GhostThief()
    {
        GameObject ghost = Instantiate(Eventsprefab[1], transform.position, Quaternion.identity);
        if (ghost != null)
        {
            ghost.GetComponent<Ghost>().Appear();
            UISFX.Instance.PlayGhostlySound();
        }
        MarketInventory.Instance.Disappear();
        MarketInventory.Instance.UpdateMenu();
    }


    private void SandwormInvasion(float interval , float totalTime)
    {
        var holemanager = GetComponent<Holemanager>();
        holemanager.SandwormEvent(interval, totalTime);
    }

    private void UnstablePortal(float interval, float totalTime)
    {
        if (Eventsprefab.Length == 0) return;
        GameObject crack = Instantiate(Eventsprefab[0], new Vector3(2f, 2f, 0f), Quaternion.identity); // location needs to adjust
        crack.GetComponent<Crack>().ExpandCrack(interval, totalTime);
    }
    

    private IEnumerator PlayTimedropMultiple(float interval, float totalDuration)
    {

        float count = totalDuration / interval;
        for (int i = 0; i < count; i++)
        {
            GameObject timedrop = Instantiate(
                Eventsprefab[2],
                new Vector3(0f, 10f, 0f),
                Quaternion.identity
            );

            if (timedrop != null)
            {
                Timedrop drop = timedrop.GetComponent<Timedrop>();
                drop.PlayWaterDrop(drop.startposition, drop.hitposition);
            }

            MarketInventory.Instance.UpdateMenu();

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator LargeCoin(float totalDuration)
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

    private IEnumerator Goblin(float interval, float totalDuration)
    {
        var spawner = FindFirstObjectByType<CustomerSpawner>();
        Vector3 pos = spawner.gameObject.transform.position;
        float count = totalDuration / interval;
        
        if (spawner != null)
        {   
            for (int i = 0 ; i < count; i++)
            {
                GameObject goblin = Instantiate(Eventsprefab[3], pos, Quaternion.identity);
                goblin.GetComponent<GoblinStateManager>().ExitPoint = spawner.gameObject.transform;
                yield return new WaitForSeconds(interval);
            }
        }
    }

    private IEnumerator Transparent(float totalDuration)
    {
        var spawner = FindFirstObjectByType<CustomerSpawner>();
        Debug.Log(spawner);
        if (spawner != null)
        {
            spawner.Trans = true;
        }
        yield return new WaitForSeconds(totalDuration);
        spawner.Trans = false;
    }
}
