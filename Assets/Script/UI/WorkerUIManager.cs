using System.Collections.Generic;
using UnityEngine;

public class WorkerUIManager : MonoBehaviour
{
    private GameObject[] workerInfos;

    private HashSet<int> occupiedUI = new HashSet<int>();


    private const string WORK_INFO_UI_TAG = "BackWorkerUI";  // FrontWorkerUi BackWorkerUi



    void Awake()
    {
        workerInfos = GameObject.FindGameObjectsWithTag(WORK_INFO_UI_TAG);

    }

    public void FillWorkerInfoUI(GameObject worker)
    {
        for (int i = 0; i < workerInfos.Length; i++)
        {
            if (!occupiedUI.Contains(i))
            {
                var uiObj = workerInfos[i];
                uiObj.GetComponent<WorkInfoUI>().SetWorker(worker);
                uiObj.GetComponent<WorkInfoUI>().Init();
                occupiedUI.Add(i);
                break;
            }
        }
    }
}