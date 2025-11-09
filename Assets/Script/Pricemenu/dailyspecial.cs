using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dailyspecial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickone1;
    [SerializeField] private TextMeshProUGUI pickone2;
    [SerializeField] private TextMeshProUGUI pickone3;

    [SerializeField] private Button chooseone1;
    [SerializeField] private Button chooseone2;
    [SerializeField] private Button chooseone3;

    private string originalname1;
    private string originalname2;
    private string originalname3;
    private PriceEditor priceeditor;

    private int preindex = 0;

    private void Start()
    {
        // 記住原始名稱
        originalname1 = pickone1.text;
        originalname2 = pickone2.text;
        originalname3 = pickone3.text;

        // 給按鈕綁定事件
        chooseone1.onClick.AddListener(() => SetSpecial(1));
        chooseone2.onClick.AddListener(() => SetSpecial(2));
        chooseone3.onClick.AddListener(() => SetSpecial(3));

        priceeditor = FindFirstObjectByType<PriceEditor>();
    }

    private void SetSpecial(int index)
    { 
        if (preindex == index) return;
        // 全部恢復原名
        pickone1.text = originalname1;
        pickone2.text = originalname2;
        pickone3.text = originalname3;


        // 指定的那個加 (*)
        switch (index)
        {   
            case 1: pickone1.text = originalname1 + " (*)"; 
                    //PriceEditor.price1 = Mathf.FloorToInt(PriceEditor.price1 * 0.8f);
                    //priceeditor.IntPriceChange(priceeditor.PriceField1, PriceEditor.price1);
                    break;
            case 2: pickone2.text = originalname2 + " (*)"; 
                    //PriceEditor.price2 = Mathf.FloorToInt(PriceEditor.price2 * 0.8f);
                    //priceeditor.IntPriceChange(priceeditor.PriceField2, PriceEditor.price2);
                    break;
            case 3: pickone3.text = originalname3 + " (*)";
                    //PriceEditor.price3 = Mathf.FloorToInt(PriceEditor.price3 * 0.8f);
                    //priceeditor.IntPriceChange(priceeditor.PriceField3, PriceEditor.price3);
                    break;
        }


        //switch (preindex)
        //{
        //    case 1: PriceEditor.price1 = Mathf.FloorToInt(PriceEditor.price1 * 1.25f); 
        //        priceeditor.IntPriceChange(priceeditor.PriceField1, PriceEditor.price1); 
        //        break;
        //    case 2: PriceEditor.price2 = Mathf.FloorToInt(PriceEditor.price2 * 1.25f); 
        //        priceeditor.IntPriceChange(priceeditor.PriceField2, PriceEditor.price2); 
        //        break;
        //    case 3: PriceEditor.price3 = Mathf.FloorToInt(PriceEditor.price3 * 1.25f); 
        //        priceeditor.IntPriceChange(priceeditor.PriceField3, PriceEditor.price3); 
        //        break;
        //}

        preindex = index;

        Debug.Log("特餐更換為：" + index + " 號餐");
    }
}