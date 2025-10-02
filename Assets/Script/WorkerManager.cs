using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance { get; private set; }
    [SerializeField] private GameObject[] chefs;
    // [SerializeField] private GameObject[] waiters;


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

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnChef(int idx)
    {
        GameObject cooker = Instantiate(chefs[idx], transform.position, Quaternion.identity);
        cooker.GetComponent<Cooker>().SetCookIdx(idx);
        return cooker;

    }

}
