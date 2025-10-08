using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }

    [SerializeField] private GameObject[] foodPrefabs; // assign prefabs in inspector
    public GameObject[] FoodPrefabs => foodPrefabs;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < foodPrefabs.Length; i++)
        {
            GameObject obj = foodPrefabs[i];
            obj.GetComponent<PickUpV2>().FoodIdx = i;
        }
    }

    // Spawn a random food for the customer
    public int RandomSpawnForCustomer(GameObject customer)
    {
        if (foodPrefabs.Length == 0) return -1;

        int index = Random.Range(0, foodPrefabs.Length);

        GameObject newPickup = Instantiate(foodPrefabs[index], customer.transform.position, Quaternion.identity);
        newPickup.transform.SetParent(customer.transform);
        newPickup.transform.localPosition = new Vector3(0, 2f, 0);
        customer.GetComponent<Holding>().SetHoldingItem(newPickup);

        return index;
    }

    public void SpawnForPlayer(int idx, Vector3 spawnPos)
    {
        if (foodPrefabs.Length == 0 || idx < 0 || idx >= foodPrefabs.Length) return;

        GameObject newPickup = Instantiate(foodPrefabs[idx], spawnPos, Quaternion.identity);
        newPickup.GetComponent<PickUpV2>().FoodIdx = idx;
    }

}

