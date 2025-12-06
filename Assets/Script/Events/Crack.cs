using System;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class Crack : MonoBehaviour
{
    private float interval = 0;
    private float existingtime = 0;
    private float lasttime = 0;
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;
    private QueueSystem qs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        qs = FindFirstObjectByType<QueueSystem>();
    }

    public void ExpandCrack(float inter, float existing)
    {
        gameObject.transform.localScale = Vector3.zero;

        gameObject.transform.DOScale(new Vector3(4f, 2f, 1f), 1.0f)
        .SetEase(Ease.OutBack);

        interval = inter;
        lasttime = inter;
        existingtime = existing; 
    }
    // Update is called once per frame
    
    void Update()
    {   
        existingtime -= Time.deltaTime;
        lasttime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && lasttime <= 0 && existingtime >= interval)
        {
            FakeSpawnCustomer();
            lasttime = interval;
        }

        if (existingtime < 1)
        {
            CloseCrack();
        }
    }

    private void FakeSpawnCustomer()
    {
        int Index = UnityEngine.Random.Range(0, 6);
        if (Index != 5)
        {
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.CustomerAnimation(RealCustomer);
            Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(Index);
            CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
            Energy Energy = Custom.GetComponent<Energy>();
            Custom.Attributedizzyprop(Index);
            Energy.UpdateEnergy(Index);
        }
        else
        {
            int randomindex = UnityEngine.Random.Range(0, 5);
            GameObject Customer = spawnedCustomer[Index];
            GameObject RealCustomer = Instantiate(Customer, transform.position, Quaternion.identity);
            CustomerStateManager Custom = RealCustomer.GetComponent<CustomerStateManager>();
            Custom.CustomerAnimation(RealCustomer);
            Custom.customerProperty = CustomerPropertyManager.Instance.GetPropertyByTypeNumber(randomindex);
            CustomerPropertyManager.Instance.Updateprop(Custom.customerProperty);
            Energy Energy = Custom.GetComponent<Energy>();
            Custom.Attributedizzyprop(randomindex);
            Energy.UpdateEnergy(randomindex);
        }
    }

    private void CloseCrack()
    {
        Debug.Log("CloseCrack called on " + gameObject.name);
        DOTween.Kill(gameObject);
        Destroy(gameObject);
    }
}
