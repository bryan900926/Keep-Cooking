using System.Collections;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
    [Header("Energy")]
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyDecay;
    private float currentEnergy;

    private Coroutine drinkCoroutine;

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
        energyDecay = 8 - customerProperty.satisfactory;
    }

    public IEnumerator DrinkCoroutine()
    {
        isReplenishing = true;

        while (currentEnergy < maxEnergy)
        {
            Replenish(maxEnergy * 0.1f);
            yield return _waitForSeconds1;
        }

        isReplenishing = false;
    }

    public void FeedDrink()
    {
        if (drinkCoroutine != null)
        {
            StopCoroutine(drinkCoroutine);
        }
        drinkCoroutine = StartCoroutine(DrinkCoroutine());
    }
}
