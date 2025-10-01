using UnityEngine;
using UnityEngine.UI;
using TMPro;  // if you're using TMP InputField

public class WorkInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image workerImage;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button cookButton;

    private WorkerData workerData;


    // Call this when creating/initializing the UI
    public void Init()
    {
        if (workerData != null)
        {
            workerImage.sprite = workerData.image;
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
        Debug.Log(workerData.id + " will cook " + input);
        // You can send this back to a manager system
    }

    public void setWorkerData(WorkerData data)
    {
        workerData = data;
    }
}
