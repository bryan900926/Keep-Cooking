using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyDecay;
    private float currentEnergy;

    public float CurrentEnergy
    {
        get => currentEnergy;
        set => currentEnergy = value;
    }
    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }

    private bool isReplenishing = false;
    public bool IsReplenishing
    {
        get => isReplenishing;
        set => isReplenishing = value;
    }

    private float surviveTime;
    public float SurviveTime
    {
        get => surviveTime;
    }

    public float EnergyRatio
    {
        get => currentEnergy / maxEnergy;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnergy = maxEnergy;
        surviveTime = maxEnergy / energyDecay;
    }

    public void UpdateEnergy(float delta)
    {
        if (isReplenishing) return;
        currentEnergy -= energyDecay * delta;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }

    public void Reset()
    {
        currentEnergy = maxEnergy;
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }

    public void Replenish(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }

    public void UpdateEnergy(int index)
    {
        CustomerProperty customerProperty = CustomerPropertyManager.Instance.customerProperties[index];
        maxEnergy = customerProperty.energy;
        energyDecay = 7 * ( 1 - customerProperty.satisfactory/8); 
    }


}
