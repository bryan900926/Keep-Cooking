using UnityEngine;
using TMPro;

public class PriceEditor : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] priceFields;
    [SerializeField] private float[] menuPrices;

    private float[] initialPrices;

    public static PriceEditor Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < menuPrices.Length; i++)
        {
            TMP_InputField field = priceFields[i];
            if (field != null)
            {
                SetupPriceField(field, menuPrices[i], i);
            }
        }
        initialPrices = (float[])menuPrices.Clone();
    }

    private void SetupPriceField(TMP_InputField field, float defaultValue, int index)
    {
        field.contentType = TMP_InputField.ContentType.IntegerNumber;
        field.text = defaultValue + "\u0024";

        field.onEndEdit.AddListener((value) => OnPriceChanged(field, value, index));
        field.onSelect.AddListener((_) => OnClickPriceButton(index));
    }

    private void OnPriceChanged(TMP_InputField field, string value, int index)
    {
        if (float.TryParse(value, out float newValue))
        {
            menuPrices[index] = newValue;
            field.text = newValue + "\u0024";
        }
    }

    private void OnClickPriceButton(int index)
    {
        var field = priceFields[index];
        field.text = field.text.Replace("\u0024", "");
    }

    public void IntPriceChange(TMP_InputField field, int value)
    {
        field.text = value.ToString() + "\u0024";
    }

    public float GetPriceForCustomer(int foodIdx)
    {
        if (foodIdx < 0 || foodIdx >= menuPrices.Length)
        {
            Debug.LogWarning("Invalid food index: " + foodIdx);
            return -1;
        }

        float timeRatio = TimeManager.Instance.GetRemainingTimeRatio();
        return Mathf.Lerp(initialPrices[foodIdx], 0, 1 - timeRatio) + 20; // price decreases over 500 sec
    }

    public float GetSellingPrice(int foodIdx)
    {
        if (foodIdx < 0 || foodIdx >= menuPrices.Length)
        {
            Debug.LogWarning("Invalid food index: " + foodIdx);
            return -1;
        }
        return menuPrices[foodIdx];
    }

}