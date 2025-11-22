using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image workerImage;
    [SerializeField] private Button buttom;
    [SerializeField] private ProgressBar progressBar;

    [SerializeField] private TextMeshProUGUI levelUpInfoText;

    private GameObject worker;

    private Level workerLevel;

    void Start()
    {
        if (!workerImage || !buttom || !progressBar)
        {
            Debug.LogError("Please attach the property for WorkInfo script");
        }

    }

    private void OnLevelUpButtonClicked()
    {
        if (worker == null) return;
        workerLevel.LevelUp();
        int currentLevel = workerLevel.LevelValue;
        levelUpInfoText.text = $"Lv{currentLevel} → Lv{currentLevel + 1}";
    }

    public void SetWorker(GameObject workerObj, bool isCooker = true)
    {
        worker = workerObj;

        if (worker != null)
        {
            if (isCooker)
            {
                var workerManager = worker.GetComponent<ChefStateManager>();
                workerImage.sprite = workerManager.WorkerData.image;
            }
            else
            {
                var workerManager = worker.GetComponent<WaiterStateManager>();
                workerImage.sprite = workerManager.WorkerData.image;
            }
            InitWorkerInfo();
        }
    }
    public void ClearUI()
    {
        workerImage.sprite = null;
        buttom.onClick.RemoveAllListeners();
    }
    private void InitWorkerInfo()
    {
        buttom.onClick.RemoveAllListeners();
        workerLevel = worker.GetComponentInChildren<Level>();
        buttom.onClick.AddListener(OnLevelUpButtonClicked);
        int currentLevel = workerLevel.LevelValue;
        levelUpInfoText.text = $"Lv{currentLevel} → Lv{currentLevel + 1}";
    }
    private void OnDestroy()
    {
        buttom.onClick.RemoveAllListeners();
    }
}
