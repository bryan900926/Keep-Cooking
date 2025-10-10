using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;
    [SerializeField] private float maxEnergy = 50f;
    [SerializeField] private float energyDecay = 10f;
    private float currentEnergy;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnergy = maxEnergy;
    }

    public void UpdateEnergy(float delta)
    {
        currentEnergy -= energyDecay * delta;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }


}
