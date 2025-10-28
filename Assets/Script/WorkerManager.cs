using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance { get; private set; }
    [SerializeField] private GameObject[] chefs;
    [SerializeField] private GameObject[] waiters;
    public GameObject[] Waiters => waiters;


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


    public GameObject SpawnChef(int cookIdx, int workerIdx = -1)
    {
        if (workerIdx == -1)
        {
            workerIdx = cookIdx;
        }
        GameObject cooker = Instantiate(chefs[workerIdx], transform.position, Quaternion.identity);
        cooker.GetComponent<ChefStateManager>().Initialize(cookIdx);
        return cooker;

    }

    public GameObject SpawnWaiter(int idx)
    {
        GameObject waiter = Instantiate(waiters[idx], transform.position, Quaternion.identity);
        return waiter;
    }

}
