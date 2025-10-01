using UnityEngine;

public class WorkManager : MonoBehaviour
{
    [SerializeField] private WorkerData[] workers;

    private GameObject[] workerInfos;

    private const string FRONT_WORK_INFO_UI_TAG = "FrontWorkerUI";
    private const string BACK_WORK_INFO_UI_TAG = "BackWorkerUI";



    void Start()
    {
        FillWorkerInfoUI(FRONT_WORK_INFO_UI_TAG);
        FillWorkerInfoUI(BACK_WORK_INFO_UI_TAG);
    }

    private void FillWorkerInfoUI(string workerType)
    {
        workerInfos = GameObject.FindGameObjectsWithTag(workerType);
        for (int i = 0; i < workerInfos.Length; i++)
        {
            var workInfoUI = workerInfos[i].GetComponent<WorkInfoUI>();
            workInfoUI.setWorkerData(workers[i]);
            if (workInfoUI != null && i < workers.Length)
            {
                workInfoUI.Init();
            }
        }
    }
}