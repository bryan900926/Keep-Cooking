using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class FrontControl : MonoBehaviour
{
    public static FrontControl Instance { get; private set; }

    private HashSet<int> occupiedWaiters = new HashSet<int>();
    private Dictionary<int, int> mapper = new Dictionary<int, int>();

    public Dictionary<int, int> Mapper => mapper;

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
        StartCoroutine(InitAfterUIReady());
    }

    private IEnumerator InitAfterUIReady()
    {
        // Wait until UI manager instance is initialized
        yield return new WaitUntil(() => FrontWorkerUIManager.Instance != null);

        for (int i = 0; i < 1; i++)
        {
            AssignTask(i);
            occupiedWaiters.Add(i);
        }
    }

    void AssignTask(int waiterIdx)
    {
        if (occupiedWaiters.Contains(waiterIdx)) return;
        GameObject waiterObj = WorkerManager.Instance.SpawnWaiter(waiterIdx);
        int uiIdx = FrontWorkerUIManager.Instance.FillWorkerInfoUI(waiterObj);
        if (uiIdx != -1)
        {
            mapper[waiterIdx] = uiIdx;
        }
    }
}
