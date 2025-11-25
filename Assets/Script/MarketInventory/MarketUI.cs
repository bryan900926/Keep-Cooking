using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private Transform[] trans;
    [SerializeField] private TextMeshProUGUI TOTAL;
    public static MarketUI Instance;

    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        RefreshUI(1);
        MarketInventory.Instance.page = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            UpclickButtom();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            DownclickButtom();
        }
    }

    public void UpclickButtom()
    {
        MarketInventory.Instance.page %= 3;
        MarketInventory.Instance.page += 1;
        RefreshUI(MarketInventory.Instance.page);
    }

    public void DownclickButtom()
    {
        if (MarketInventory.Instance.page <= 1){
            MarketInventory.Instance.page = 4;
        }
        MarketInventory.Instance.page -= 1;
        RefreshUI(MarketInventory.Instance.page);
    }

    public void RefreshUI(int page)
    {
        int startIndex = 4 * (page - 1);
        int endIndex = Mathf.Min(startIndex + 4, MarketInventory.Instance.slots.Count);

        for (int j = 0; j < 4; j++)
        {
            Transform slot = trans[j];

            int dataIndex = startIndex + j;

            if (dataIndex < MarketInventory.Instance.slots.Count)
            {
                var slotData = MarketInventory.Instance.slots[dataIndex];
                if (slotData == null || slotData.item == null) continue;

                Image frame = slot.Find("frame").GetComponent<Image>();
                Image food = slot.Find("ingredient").GetComponent<Image>();
                TextMeshProUGUI nameText = slot.Find("name").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI stockText = slot.Find("stock").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI limitText = slot.Find("limit").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI priceText = slot.Find("price").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI totalText = slot.Find("total").GetComponent<TextMeshProUGUI>();
                TMP_InputField numberText = slot.Find("number").GetComponent<TMP_InputField>();

                int totalprice = slotData.price * slotData.Currentcount;

                frame.enabled = true;
                food.enabled = true;
                food.sprite = slotData.item.image;
                nameText.text = slotData.item.Name;
                priceText.text = slotData.price.ToString();
                stockText.text = slotData.amount.ToString();
                limitText.text = slotData.limited.ToString();
                totalText.text = totalprice.ToString();
                numberText.text = slotData.Currentcount.ToString();
            }
            else
            {
                Image frame = slot.Find("frame").GetComponent<Image>();
                Image food = slot.Find("ingredient").GetComponent<Image>();
                TextMeshProUGUI nameText = slot.Find("name").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI stockText = slot.Find("stock").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI limitText = slot.Find("limit").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI priceText = slot.Find("price").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI totalText = slot.Find("total").GetComponent<TextMeshProUGUI>();
                TMP_InputField numberText = slot.Find("number").GetComponent<TMP_InputField>();

                food.sprite = null;
                frame.enabled = false;
                food.enabled = false;
                nameText.text = "";
                priceText.text = "";
                stockText.text = "";
                limitText.text = "";
                totalText.text = "";
                numberText.text = "";
            }
        }

        int totalSum = 0;
        totalSum = MarketInventory.Instance.AllTotal();
        TOTAL.text = totalSum.ToString();
    }
}
