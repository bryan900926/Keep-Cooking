using System;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;
    [SerializeField] private DoorController doorController;
    [SerializeField] private TextMeshProUGUI[] ratio;
    public float[] spawnIntervals;

    private float[] originalpdf = new float[7];
    private float[] pdf;
    
    private QueueSystem qs;
    private float spawnedTime = 0;
    public bool LargeCoin;
    public bool Trans = false;
    private Transparency trans;
    private int random;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pdf = (float[])originalpdf.Clone();
        qs = lining.GetComponent<QueueSystem>();
        trans = GetComponent<Transparency>();
        spawnedTime = UnityEngine.Random.Range(spawnIntervals[0], spawnIntervals[1]);
        Initpdf();
        UpdateDistribution();
        random = UnityEngine.Random.Range(0, 6);
    }

    // Update is called once per 
    void Update()
    {
        spawnedTime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && spawnedTime <= 0)
        {
            SpawnCustomer(LargeCoin, Trans, random);
            spawnedTime = UnityEngine.Random.Range(spawnIntervals[0], spawnIntervals[1]);
        }

        if (qs.availSeats.Count > 0 && CustomerPropertyManager.Instance.NiceCustomer >= 25)
        {
            SpecialSpawner(true, LargeCoin, Trans);
        }
        else if (qs.availSeats.Count > 0 && CustomerPropertyManager.Instance.BadCustomer >= 10)
        {
            SpecialSpawner(false, LargeCoin, Trans);
        }
    }

    public void Initpdf()
    {
        for (int i = 0; i < spawnedCustomer.Length; i++)
        {
            originalpdf[i] = CustomerPropertyManager.Instance.customerProperties[i].ratio;
            ratio[i].text = (100 * originalpdf[i]).ToString("F0") + "%";
        }
    }

    public void UpdateDistribution()
    {
        for (int i = 0; i < spawnedCustomer.Length; i++)
        {
            pdf[i] = originalpdf[i];
            switch (i)
            {
                case 0:
                    pdf[i] -= ReputationSystem.Instance.GetReputationRatio() / 2.5f;
                    ratio[i].text = (100 * pdf[i]).ToString("F0") + "%";
                    break;
                case 1:
                    pdf[i] -= ReputationSystem.Instance.GetReputationRatio() / 5f;
                    ratio[i].text = (100 * pdf[i]).ToString("F0") + "%";
                    break;
                case 2:
                    pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 20f;
                    ratio[i].text = (100 * pdf[i]).ToString("F0") + "%";
                    break;
                case 3:
                case 4: // 3 和 4 的邏輯一樣，可以合併寫
                    pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 10f;
                    ratio[i].text = (100 * pdf[i]).ToString("F0") + "%";
                    break;
                case 5:
                    pdf[i] += ReputationSystem.Instance.GetReputationRatio() / 5f;
                    ratio[i].text = (100 * pdf[i]).ToString("F0") + "%";
                    break;
            }
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

    void SpecialSpawner(bool state, bool Largecoin, bool Trans)
    {
        UpdateDistribution();
        TriggerDoorOpen();
        GameObject Customer = spawnedCustomer[6];
        GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
        if (Trans) trans.StartInvisible(RealCustomer); 
        CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>(); 
        Custom.trans = Trans;
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

    }

    void SpawnCustomer(bool Largecoin, bool Trans, int random)
    {   
        CustomerPropertyManager.Instance.TotalCustomer += 1;
        UpdateDistribution();
        int Index = RandomIndex(pdf);
        TriggerDoorOpen();
        if (Index != 5)
        {
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            if (Trans) trans.StartInvisible(RealCustomer);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.trans = Trans;
            Custom.largecoin = Largecoin;
            Energy Energy = Custom.GetComponent<Energy>();
            if (Trans)
            {
                trans.StartInvisible(RealCustomer);
                Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(random);
                CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
                Custom.Attributeprop(random);
                Energy.UpdateEnergy(random);

            }
            else
            {
                Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(Index);
                CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
                Custom.Attributeprop(Index);
                Energy.UpdateEnergy(Index);
            }
        }
        else
        {
            int randomindex = UnityEngine.Random.Range(0, 5);
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.trans = Trans;
            Custom.largecoin = Largecoin;
            Energy Energy = Custom.GetComponent<Energy>();
            if (Trans)
            {
                trans.StartInvisible(RealCustomer);
                Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(random);
                CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
                Custom.Attributeprop(random);
                Energy.UpdateEnergy(random);

            }
            else
            {
                Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(randomindex);
                CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
                Custom.Attributeprop(randomindex);
                Energy.UpdateEnergy(randomindex);
            }
                
        }
        
    }

    private void TriggerDoorOpen()
    {
        doorController?.TriggerDoorOpen();
    }


}
