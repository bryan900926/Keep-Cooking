using UnityEngine;

public class OilSpot : MonoBehaviour
{
    private OilSpotSpawner spawner;

    public void SetSpawner(OilSpotSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveSpot();
        }
    }
}
