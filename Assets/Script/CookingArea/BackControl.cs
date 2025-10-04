using System.Collections.Generic;
using UnityEngine;

public class BackControl : MonoBehaviour
{
    public static BackControl Instance { get; private set; }
    private GameObject[] cookers;

    public GameObject[] GetCookers => cookers;

    private HashSet<int> occupiedCookers = new HashSet<int>();

    private Dictionary<int, int> mapper = new Dictionary<int, int>(); // map from cooker idx to UI idx

    public Dictionary<int, int> Mapper => mapper;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        cookers = GameObject.FindGameObjectsWithTag("Cooking");
        Instance = this;
        for (int i = 0; i < cookers.Length; i++)
        {
            AssignTask(i);
            occupiedCookers.Add(i);
        }
    }


    void AssignTask(int cookIdx) // cookIdx: the kitchen idx
    {
        if (occupiedCookers.Contains(cookIdx) || cookIdx >= cookers.Length) return;
        GameObject cookerObj = WorkerManager.Instance.SpawnChef(cookIdx);
        int uiIdx = BackWorkerUIManager.Instance.FillWorkerInfoUI(cookerObj);
        if (uiIdx != -1)
        {
            mapper[cookIdx] = uiIdx;
        }

    }
}
