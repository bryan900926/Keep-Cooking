using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;

    private QueueSystem qs;
    private float spawnedTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        qs = lining.GetComponent<QueueSystem>();
        spawnedTime = Random.Range(3, 5);

    }

    // Update is called once per frame
    void Update()
    {
        spawnedTime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && spawnedTime <= 0)
        {
            SpawnCustomer();
            spawnedTime = Random.Range(3, 5);
        }
    }

    void SpawnCustomer()
    {
        int randomIndex = Random.Range(0, spawnedCustomer.Length);
        Instantiate(spawnedCustomer[randomIndex], transform.position, Quaternion.identity);
    }
}
