using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image workerImage;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button cookButton;
    [SerializeField] private ProgressBar progressBar;

    private GameObject worker;

    void Start()
    {
        if (!workerImage || !inputField || !cookButton || !progressBar)
        {
            Debug.LogError("Please attach the property for WorkInfo script");
        }
    }

    private void OnCookButtonClicked()
    {
        if (worker == null) return;

        string input = inputField.text;
        if (string.IsNullOrEmpty(input)) return;

        if (!int.TryParse(input, out int idx)) return;

        Cooker cooker = worker.GetComponent<Cooker>();
        ChefStateManager chefStateManager = worker.GetComponent<ChefStateManager>();

        // Check if worker is at destination and not already cooking
        if (Vector2.Distance(worker.transform.position, cooker.Destination.position) <= 1f)
        {
            float cookingTime = chefStateManager.EnableCooking(idx);
            if (cookingTime > 0)
            {
                progressBar.StartTimer(cookingTime);
            }
        }
    }


    public void SetWorker(GameObject workerObj)
    {
        worker = workerObj;

        if (worker != null)
        {
            var cooker = worker.GetComponent<Cooker>();
            workerImage.sprite = cooker.WorkerData.image;

            cookButton.onClick.RemoveAllListeners();
            cookButton.onClick.AddListener(OnCookButtonClicked);

        }
    }
    public void ClearUI()
    {
        workerImage.sprite = null;
        inputField.text = "";
        cookButton.onClick.RemoveAllListeners();
    }
}
