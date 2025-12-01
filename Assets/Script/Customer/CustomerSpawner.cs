using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;

    [SerializeField] private float[] spawnIntervals;

    private QueueSystem qs;
    private float spawnedTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        qs = lining.GetComponent<QueueSystem>();
        spawnedTime = Random.Range(spawnIntervals[0], spawnIntervals[1]);
    }

    // Update is called once per frame
    void Update()
    {
        spawnedTime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && spawnedTime <= 0)
        {
            SpawnCustomer();
            spawnedTime = Random.Range(spawnIntervals[0], spawnIntervals[1]);
        }
    }

    void SpawnCustomer()
    {
        int Index = Random.Range(0, spawnedCustomer.Length);
        GameObject Customer = spawnedCustomer[Index];
        GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
        CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
        Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(Index);
        Energy Energy = Custom.GetComponent<Energy>();
        CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);

        Custom.Attributeprop(Index);
        Energy.UpdateEnergy(Index);

    }


}
