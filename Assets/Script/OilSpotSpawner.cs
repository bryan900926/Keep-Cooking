using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OilSpotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject oilPrefab;
    [SerializeField] private int numberOfSpots = 10;
    [SerializeField] private float spotRadius = 2f; // radius of oil spot
    [SerializeField] private LayerMask obstacleLayer; // assign floor objects here

    private BoxCollider2D spawnArea;

    private float spawnInterval;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        spawnInterval = Random.Range(5f, 10f);
    }



    void Update()
    {
        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0)
        {
            SpawnOilSpots();
            spawnInterval = Random.Range(5f, 10f);
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
            Instantiate(oilPrefab, randomPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);
        }
        else
        {
            Debug.LogWarning("Failed to find free spot for oil after 100 attempts.");
        }
    }


}


