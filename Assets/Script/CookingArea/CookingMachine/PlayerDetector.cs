using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private CookingMachineStateManager stateManager;

    private const string PLAYER_TAG = "Player";

    void Start()
    {
        stateManager = GetComponent<CookingMachineStateManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stateManager.CurrentState is CookingMachineOnFireState && other.CompareTag(PLAYER_TAG) && other.GetComponent<Holding>().HoldingItem.name == "WaterBucket")
        {
            Debug.Log("Player entered detection area!");
            stateManager.SetBackToNormal();
        }
    }
}
