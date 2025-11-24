using System;
using UnityEngine;

public class ReputationSystem : MonoBehaviour
{
    public static ReputationSystem Instance { get; private set; }
    float reputationLevel = 50f;
    readonly float maxReputation = 100f;

    public static Action OnReputationChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void IncreaseReputation(float amount)
    {
        reputationLevel += amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, maxReputation);
        OnReputationChanged?.Invoke();
    }
    public void DecreaseReputation(float amount)
    {
        reputationLevel -= amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, maxReputation);
        OnReputationChanged?.Invoke();
    }

    public float GetReputationRatio()
    {
        return reputationLevel / maxReputation;
    }
}
