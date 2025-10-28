using UnityEngine;
using UnityEngine.UI;

public class FloatingEnergyBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateEnergy(float ratio)
    {
        slider.value = ratio;
    }
}
