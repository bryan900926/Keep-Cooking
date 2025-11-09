using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance;

    private readonly Dictionary<int, PriorityQueue<OrderInfo>> tableToOrders = new(); // for waiter to deliver orders

    private PriorityQueue<OrderInfo> chefOrders = new(orderInfo => orderInfo.CustomerObj == null || orderInfo.CustomerObj.GetComponent<CustomerStateManager>().CurrentState is not CustomerWaitFoodState);

    public PriorityQueue<OrderInfo> ChefOrders { get => chefOrders; set => chefOrders = value; }
    private void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public OrderInfo GetHighestPriorityOrder(int foodIndex)
    {
        if (tableToOrders.ContainsKey(foodIndex))
            return tableToOrders[foodIndex].Dequeue();
        return null;
    }

    public void AddNewOrder(int foodIndex, int tableIdx, float duration, GameObject customerObj = null)
    {
        if (!tableToOrders.ContainsKey(foodIndex))
        {
            tableToOrders[foodIndex] = new PriorityQueue<OrderInfo>(orderInfo => orderInfo.CustomerObj == null || orderInfo.CustomerObj.GetComponent<CustomerStateManager>().CurrentState is not CustomerWaitFoodState);
        }
        OrderInfo orderInfo = new OrderInfo(foodIndex, duration, tableIdx, customerObj);
        tableToOrders[foodIndex].Enqueue(orderInfo, orderInfo.EndTime);
        if (foodIndex != -1 && foodIndex < Menu.Instance.FoodPrefabs.Length - 1)
            chefOrders.Enqueue(orderInfo, orderInfo.EndTime);
    }

    public void AddFailOrder(OrderInfo orderInfo, bool isWaiterOrder)
    {
        if (orderInfo.FoodIdx < Menu.Instance.FoodPrefabs.Length - 1)
            chefOrders.Enqueue(orderInfo, orderInfo.EndTime);
        if (isWaiterOrder)
        {
            tableToOrders[orderInfo.FoodIdx].Enqueue(orderInfo, orderInfo.EndTime);
        }
    }

    public List<OrderInfo> GetOrderForChef(int maxOrders = 1)
    {
        List<OrderInfo> orderInfos = new();
        int cnt = 0;
        while (cnt < maxOrders && chefOrders.Count > 0)
        {
            orderInfos.Add(chefOrders.Dequeue());
            cnt++;
        }
        return orderInfos;
    }

}

public class OrderInfo
{
    public int FoodIdx { get; private set; }
    public float duration { get; private set; }

    public int TableIdx { get; private set; }

    public float EndTime;

    public GameObject CustomerObj;

    public OrderInfo(int foodIdx, float duration, int tableIdx = -1, GameObject customerObj = null)
    {
        this.FoodIdx = foodIdx;
        this.duration = duration;
        this.TableIdx = tableIdx;
        this.EndTime = Time.time + duration;
        this.CustomerObj = customerObj;
    }
}