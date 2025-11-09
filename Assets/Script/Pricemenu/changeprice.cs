using UnityEngine;
using TMPro;

public class PriceEditor : MonoBehaviour
{
    [SerializeField] private TMP_InputField priceField1;
    [SerializeField] private TMP_InputField priceField2;
    [SerializeField] private TMP_InputField priceField3;

    public TMP_InputField PriceField1 => priceField1;
    public TMP_InputField PriceField2 => priceField2;
    public TMP_InputField PriceField3 => priceField3;

    public static int price1 = 10;
    public static int price2 = 10;
    public static int price3 = 10;


    private void Start()
    {
        SetupPriceField(priceField1, price1, 1);
        SetupPriceField(priceField2, price2, 2);
        SetupPriceField(priceField3, price3, 3);
    }

    private void SetupPriceField(TMP_InputField field, int defaultValue, int index)
    {
        field.contentType = TMP_InputField.ContentType.IntegerNumber;
        field.text = defaultValue + "\u0024";

        field.onEndEdit.AddListener((value) => OnPriceChanged(field, value, index));
    }

    private void OnPriceChanged(TMP_InputField field, string value, int index)
    {
        int oldValue = GetPrice(index); // 先存舊值，錯誤時回復用

        if (int.TryParse(value, out int newValue))
        {
            SetPrice(index, newValue);
            field.text = newValue + "\u0024";
            Debug.Log($"更新價格 {index} 為：" + field.text);
        }
        else
        {
            // 回填舊值
            field.text = oldValue + "\u0024";
            Debug.LogWarning("輸入不是有效數字：" + value + " 已回復舊值");
        }
    }

    private int GetPrice(int index)
    {
        switch (index)
        {
            case 1: return price1;
            case 2: return price2;
            case 3: return price3;
        }
        return 0;
    }

    private void SetPrice(int index, int newValue)
    {
        switch (index)
        {
            case 1: price1 = newValue; break;
            case 2: price2 = newValue; break;
            case 3: price3 = newValue; break;
        }
    }

    public void IntPriceChange(TMP_InputField field, int value)
    {
       field.text = value.ToString() + "\u0024";
       Debug.Log("更新價格為：" + field.text);
    }
}