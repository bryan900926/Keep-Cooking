using System;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;
    [SerializeField] private float[] spawnIntervals;

    private float[] originalpdf = new float[7];
    private float[] pdf;
    
    private QueueSystem qs;
    private float spawnedTime = 0;
    public bool LargeCoin; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pdf = (float[])originalpdf.Clone();
        qs = lining.GetComponent<QueueSystem>();
        spawnedTime = UnityEngine.Random.Range(spawnIntervals[0], spawnIntervals[1]);
        Initpdf();
        UpdateDistribution();
    }

    // Update is called once per frame
    void Update()
    {
        spawnedTime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && spawnedTime <= 0)
        {
            SpawnCustomer(LargeCoin);
            spawnedTime = UnityEngine.Random.Range(spawnIntervals[0], spawnIntervals[1]);
        }

        if (qs.availSeats.Count > 0 && CustomerPropertyManager.Instance.NiceCustomer >= 40)
        {
            SpecialSpawner(true, LargeCoin);
        }
        else if (qs.availSeats.Count > 0 && CustomerPropertyManager.Instance.BadCustomer >= 40)
        {
            SpecialSpawner(false, LargeCoin);
        }
    }

    public void Initpdf()
    {
        for (int i = 0; i < spawnedCustomer.Length; i++)
        {
            originalpdf[i] = CustomerPropertyManager.Instance.customerProperties[i].ratio;
        }
    }

    public void UpdateDistribution()
    {
        for (int i = 0; i < spawnedCustomer.Length; i++)
        {
            pdf[i] = originalpdf[i];
            if (i==0) pdf[i] -= ReputationSystem.Instance.GetReputationRatio()/2.5f;
            else if (i==1) pdf[i] -= ReputationSystem.Instance.GetReputationRatio() / 5f;
            else if (i==2) pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 20f;
            else if (i==3) pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 10f;
            else if (i==4) pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 10f;
            else if (i==5) pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 5f;
        }
    }

    public int RandomIndex(float[] weights)
    {
        float totalWeight = 0;

        foreach (float w in weights)
            totalWeight += w;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        for (int i = 0; i < weights.Length; i++)
        {
            if (randomValue < weights[i])
            {
                return i;   
            }
            randomValue -= weights[i];
        }

        return 0; 
    }

    void SpecialSpawner(bool state, bool Largecoin)
    {
        UpdateDistribution();
        GameObject Customer = spawnedCustomer[6];
        GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
        CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
        Custom.largecoin = Largecoin;
        Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(6);
        Energy Energy = Custom.GetComponent<Energy>();
        if (state)
            Custom.customerProperty = CustomerPropertyManager.Instance.Specialpropertypositive(CustomerPropertyManager.Instance.NiceCustomer);
        else
            Custom.customerProperty = CustomerPropertyManager.Instance.Specialpropertynegative(CustomerPropertyManager.Instance.BadCustomer);
        CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
        Custom.Attributeprop(6);
        Energy.UpdateEnergy(6);
        CustomerPropertyManager.Instance.NiceCustomer = 0;
        CustomerPropertyManager.Instance.BadCustomer = 0;
        spawnIntervals[0] -= 1;
        spawnIntervals[1] -= 1;

    }

    void SpawnCustomer(bool Largecoin)
    {   
        CustomerPropertyManager.Instance.TotalCustomer += 1;
        UpdateDistribution();
        int Index = RandomIndex(pdf);
        if (Index != 5)
        {
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.largecoin = Largecoin;
            Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(Index);
            Energy Energy = Custom.GetComponent<Energy>();
            CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
            Custom.Attributeprop(Index);
            Energy.UpdateEnergy(Index);
        }
        else
        {
            int randomindex = UnityEngine.Random.Range(0, 5);
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.largecoin = Largecoin;
            Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(randomindex);
            Energy Energy = Custom.GetComponent<Energy>();
            CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
            Custom.Attributeprop(randomindex);
            Energy.UpdateEnergy(randomindex);
        }
        
    }


}
