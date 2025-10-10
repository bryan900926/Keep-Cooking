using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackControl : MonoBehaviour
{
    public static BackControl Instance { get; private set; }

    private GameObject[] cookers;
    private Dictionary<int, int> mapper = new Dictionary<int, int>();

    public Dictionary<int, int> Mapper => mapper;
    public GameObject[] GetCookers => cookers;

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
        cookers = GameObject.FindGameObjectsWithTag("Cooking");
        StartCoroutine(InitializeAfterUIManagerReady());
    }

    private IEnumerator InitializeAfterUIManagerReady()
    {
        // Wait until BackWorkerUIManager instance exists
        yield return new WaitUntil(() => BackWorkerUIManager.Instance != null);

        for (int i = 0; i < cookers.Length; i++)
        {
            AssignTask(i);
        }
    }

    private int AssignTask(int cookIdx)
    {
        if (mapper.ContainsKey(cookIdx) || cookIdx >= cookers.Length) return -1;

        GameObject cookerObj = WorkerManager.Instance.SpawnChef(cookIdx);
        int uiIdx = BackWorkerUIManager.Instance.FillWorkerInfoUI(cookerObj);
        if (uiIdx != -1)
        {
            mapper[cookIdx] = uiIdx;
        }
        return uiIdx;
    }
    public void RecruitChef(GameObject customer)
    {
        for (int i = 0; i < cookers.Length; i++)
        {
            if (!mapper.ContainsKey(i))
            {
                Debug.Log($"Assigning cookIdx {i} to new chef.");
                customer.GetComponent<CustomerStateManager>().ChangeState(new CustomerToChefState(customer.GetComponent<CustomerStateManager>(), i));
                break;
            }
        }
    }
}
