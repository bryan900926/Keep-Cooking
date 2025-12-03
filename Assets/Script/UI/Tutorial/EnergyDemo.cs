using UnityEngine;

public class EnergyDemo : MonoBehaviour
{
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;

    private float currentEnergy = 100f;
    private const float maxEnergy = 100f;

    void Update()
    {
        currentEnergy -= 10f * Time.deltaTime;
        if (currentEnergy < 0f) currentEnergy = maxEnergy;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }

}