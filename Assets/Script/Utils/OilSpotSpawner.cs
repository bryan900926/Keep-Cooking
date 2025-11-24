using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OilSpotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject oilPrefab;
    [SerializeField] private int numberOfSpots = 8;
    [SerializeField] private float spotRadius = 2f; // radius of oil spot
    [SerializeField] private LayerMask obstacleLayer; // assign floor objects here

    private BoxCollider2D spawnArea;

    private float spawnInterval;
    private int currentSpots = 0;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        spawnInterval = Random.Range(8f, 10f);
    }

    public void RemoveSpot()
    {
        currentSpots--;
    }


    void Update()
    {
        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0 && currentSpots < numberOfSpots)
        {
            SpawnOilSpots();
            spawnInterval = Random.Range(15f, 20f);
        }
    }
    private void SpawnOilSpots()
    {
        Bounds bounds = spawnArea.bounds;
        int attempts = 0;

        Vector3 randomPos;
        bool validPosition = false;

        do
        {
            randomPos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                0f
            );

            // Check for collisions with obstacles
            if (!Physics2D.OverlapCircle(randomPos, spotRadius, obstacleLayer))
            {
                validPosition = true;
                break;
            }

            attempts++;
        } while (!validPosition && attempts < 100);

        if (validPosition)
        {
            GameObject spotObj = Instantiate(oilPrefab, randomPos, Quaternion.identity, transform);

            OilSpot oilSpot = spotObj.GetComponent<OilSpot>();
            currentSpots++;
            if (oilSpot != null)
                oilSpot.SetSpawner(this);
        }
        else
        {
            Debug.LogWarning("Failed to find free spot for oil after 100 attempts.");
        }
    }


}


