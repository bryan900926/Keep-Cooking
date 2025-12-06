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


    [SerializeField] private MarketEvent[] allEvents;

    public GameObject[] Eventsprefab;

    private BlackOverlayController blackOverlayController;

    private float eventmultiplier;

    private string propname;

    private bool First = true;

    [SerializeField] private int interval = 0;

    [SerializeField] private int totaltime = 0;
    readonly private Dictionary<string, Action> eventActions = new();

    private void Awake()
    {
        Instance = this;
        blackOverlayController = GetComponent<BlackOverlayController>();
    }

    void Start()
    {
        eventActions.Add("Ghostly Thief", GhostThief);
        eventActions.Add("Time-Loop Hour", () => StartCoroutine(PlayTimedropMultiple(interval, totaltime)));
        eventActions.Add("Sandworm Invasion", SandwormInvasion);
        eventActions.Add("Unstable Portal", UnstablePortal);
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
        if (allEvents.Length == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, allEvents.Length);
        MarketEvent randomEvent = allEvents[1];
        ApplyMarketEvent(randomEvent);
        currentEvent = randomEvent;
        First = false;
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


    private void SandwormInvasion()
    {
        var holemanager = GetComponent<Holemanager>();
        holemanager.SandwormEvent(2f, 50f);
    }

    private void UnstablePortal()
    {
        if (Eventsprefab.Length == 0) return;
        GameObject crack = Instantiate(Eventsprefab[0], new Vector3(2f, 2f, 0f), Quaternion.identity); // location needs to adjust
        crack.GetComponent<Crack>().ExpandCrack(3f, 15f);
    }

    private IEnumerator PlayTimedropMultiple(int interval, int totalDuration)
    {

        int count = totalDuration / interval;
        for (int i = 0; i < count; i++)
        {
            // �ͦ� timedrop
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

            // ��s���]�p�G�n�C������s�^
            MarketInventory.Instance.UpdateMenu();

            // ���U�@��
            yield return new WaitForSeconds(interval);
        }
    }
}
