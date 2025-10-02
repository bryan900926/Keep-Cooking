using UnityEngine;
using UnityEngine.UI;
using TMPro;  // if you're using TMP InputField

public class WorkInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image workerImage;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button cookButton;
    private GameObject worker;


    // Call this when creating/initializing the UI
    public void Init()
    {
        if (worker != null)
        {
            workerImage.sprite = worker.GetComponent<Cooker>().GetWorkerData().image;
            cookButton.onClick.AddListener(OnCookButtonClicked);
        }
        else
        {
            Debug.LogWarning("WorkerData is not set for WorkInfoUI.");
        }
    }

    private void OnCookButtonClicked()
    {
        string input = inputField.text;
        int.TryParse(input, out int idx);
        Cooker cooker = worker.GetComponent<Cooker>();
        if (Vector2.Distance(worker.transform.position, cooker.Destination.position) <= 1f)
        {
            worker.GetComponent<Chef>().EnableCooking(idx);
        }
    }


    public void SetWorker(GameObject workerObj)
    {
        worker = workerObj;
    }
}
