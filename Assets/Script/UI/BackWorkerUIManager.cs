using System.Collections.Generic;
using UnityEngine;

public class BackWorkerUIManager : MonoBehaviour
{

    public static BackWorkerUIManager Instance { get; private set; }
    private GameObject[] workerInfos;

    private HashSet<int> occupiedUI = new HashSet<int>();


    private const string WORK_INFO_UI_TAG = "BackWorkerUI";



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }
        Instance = this;
        workerInfos = GameObject.FindGameObjectsWithTag(WORK_INFO_UI_TAG);

    }

    public int FillWorkerInfoUI(GameObject worker)
    {
        for (int i = 0; i < workerInfos.Length; i++)
        {
            if (!occupiedUI.Contains(i))
            {
                var uiObj = workerInfos[i];
                uiObj.GetComponent<WorkInfoUI>().SetWorker(worker);
                occupiedUI.Add(i);
                return i;
            }
        }
        return -1;
    }

    public void RemoveWorkerInfoUI(int idx)
    {
        var uiObj = workerInfos[idx];
        uiObj.GetComponent<WorkInfoUI>().ClearUI();
    }
}