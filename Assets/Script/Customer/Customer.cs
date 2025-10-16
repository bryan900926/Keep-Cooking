using Pathfinding;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private GameObject queueObj;
    private QueueSystem qs;

    private AIDestinationSetter desSetter;
    private int liningIdx = -1;
    private int diningIdx = -1;

    private int orderedFoodIdx = -1;

    private GameObject diningObj;

    private DiningSystem ds;
    private AIPath ai;


    void Start()
    {
        queueObj = GameObject.FindGameObjectWithTag("Queue");
        qs = queueObj.GetComponent<QueueSystem>();
        ds = DiningSystem.Instance;
        desSetter = GetComponent<AIDestinationSetter>();
        ai = GetComponent<AIPath>();

    }

    void Update()
    {
        if (liningIdx == -1)
        {
            TryToWaitLine();
        }
        else if (diningIdx == -1 && GetComponent<AIPath>() && ai.reachedDestination)
        {
            TryToDine();
        }
        else if (orderedFoodIdx == -1 && diningIdx != -1 && Vector2.Distance(ds.seats[diningIdx].transform.position, transform.position) <= 0.5f)
        {
            orderedFoodIdx = Menu.Instance.RandomSpawnForCustomer(gameObject);
        }

    }

    public void TryToWaitLine()
    {
        int idx = qs.FetchAvailSeat();
        if (idx != -1)
        {
            liningIdx = idx;
            desSetter.target = qs.seats[idx].transform;
        }
    }
    public void TryToDine()
    {
        if (diningIdx != -1) return;
        int idx = ds.FetchAvailSeat();
        if (idx != -1)
        {
            qs.FreeSeat(liningIdx);
            diningIdx = idx;
            desSetter.target = ds.seats[idx].transform;
        }
    }

    public void DoneDining()
    {
        ds.FreeSeat(diningIdx);
        Destroy(transform.gameObject);
    }

    public int GetOrderedFoodIndex()
    {
        return orderedFoodIdx;
    }
}
