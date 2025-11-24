using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedCustomer;
    [SerializeField] private GameObject lining;

    [SerializeField] private float[] spawnIntervals;

    private QueueSystem qs;
    private float spawnedTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        qs = lining.GetComponent<QueueSystem>();
        spawnedTime = Random.Range(spawnIntervals[0], spawnIntervals[1]);
    }

    // Update is called once per frame
    void Update()
    {
        spawnedTime -= Time.deltaTime;
        // Debug.Log("Available Seats: " + qs.availSeats.Count);
        if (qs.availSeats.Count > 0 && spawnedTime <= 0)
        {
            SpawnCustomer();
            spawnedTime = Random.Range(spawnIntervals[0], spawnIntervals[1]) * (1f - ReputationSystem.Instance.GetReputationRatio());
        }
    }

    void SpawnCustomer()
    {
        int randomIndex = Random.Range(0, spawnedCustomer.Length);
        Instantiate(spawnedCustomer[randomIndex], transform.position, Quaternion.identity);
    }
}
