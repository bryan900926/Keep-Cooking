using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackControl : MonoBehaviour
{
    public static BackControl Instance { get; private set; }

    private GameObject[] cookers;
    private HashSet<int> occupiedCookers = new HashSet<int>();
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

    void AssignTask(int cookIdx)
    {
        if (occupiedCookers.Contains(cookIdx) || cookIdx >= cookers.Length) return;

        GameObject cookerObj = WorkerManager.Instance.SpawnChef(cookIdx);
        int uiIdx = BackWorkerUIManager.Instance.FillWorkerInfoUI(cookerObj);
        if (uiIdx != -1)
        {
            mapper[cookIdx] = uiIdx;
            occupiedCookers.Add(cookIdx); // only mark as occupied after successful assignment
        }
    }
}
