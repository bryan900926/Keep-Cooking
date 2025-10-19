using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class WorkInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image workerImage;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button buttom;
    [SerializeField] private ProgressBar progressBar;

    private GameObject worker;

    void Start()
    {
        if (!workerImage || !inputField || !buttom || !progressBar)
        {
            Debug.LogError("Please attach the property for WorkInfo script");
        }
    }

    private void OnCraftButtonClicked()
    {
        if (worker == null) return;
        Crafting.instance.SetCurrentChef(worker);
        Toggle.Instance.OpenPanel(Key.V);

    }

    private void OnCookButtonClicked()
    {
        if (worker == null) return;

        string input = inputField.text;
        if (string.IsNullOrEmpty(input)) return;

        if (!int.TryParse(input, out int idx)) return;

        ChefStateManager chefStateManager = worker.GetComponent<ChefStateManager>();

        // Check if worker is at destination and not already cooking
        if (chefStateManager.BeenToWorkingStation())
        {
            float cookingTime = chefStateManager.EnableCooking(idx);
            if (cookingTime > 0)
            {
                progressBar.StartTimer(cookingTime);
            }
        }
    }

    private void OnDeliverButtonClicked()
    {
        if (worker == null) return;

        string input = inputField.text;
        if (string.IsNullOrEmpty(input)) return;

        if (!int.TryParse(input, out int idx)) return;

        WaiterStateManager waiterStateManager = worker.GetComponent<WaiterStateManager>();
        waiterStateManager.tableIdx = idx;

    }



    public void SetWorker(GameObject workerObj, bool isCooker = true)
    {
        worker = workerObj;

        if (worker != null)
        {
            if (isCooker)
            {
                var cooker = worker.GetComponent<ChefStateManager>();
                workerImage.sprite = cooker.WorkerData.image;
            }
            else
            {
                var waiter = worker.GetComponent<WaiterStateManager>();
                workerImage.sprite = waiter.WorkerData.image;
            }

            buttom.onClick.RemoveAllListeners();
            if (isCooker)
            {
                buttom.onClick.AddListener(OnCraftButtonClicked);
            }
            else
            {
                buttom.onClick.AddListener(OnCraftButtonClicked);
            }

        }
    }
    public void ClearUI()
    {
        workerImage.sprite = null;
        inputField.text = "";
        buttom.onClick.RemoveAllListeners();
    }
}
