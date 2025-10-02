using System.Collections.Generic;
using UnityEngine;

public class BackControl : MonoBehaviour
{
    public static BackControl Instance { get; private set; }
    private GameObject[] cookers;

    public GameObject[] GetCookers => cookers;

    private HashSet<int> occupiedCookers = new HashSet<int>();

    [SerializeField] private GameObject backUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cookers = GameObject.FindGameObjectsWithTag("Cooking");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        for (int i = 0; i < cookers.Length; i++)
        {
            AssignTask(i);
            occupiedCookers.Add(i);
        }
    }


    void AssignTask(int idx)
    {
        if (occupiedCookers.Contains(idx)) return;
        GameObject cookerObj = WorkerManager.Instance.SpawnChef(idx);
        backUI.GetComponent<WorkerUIManager>().FillWorkerInfoUI(cookerObj);

    }
}
