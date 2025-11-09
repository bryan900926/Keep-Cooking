using UnityEngine;
using UnityEngine.UI;

public class ChefRecipeBtn : MonoBehaviour
{
    private Button button;
    private Image icon;
    [SerializeField] private int dishID;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color darkColor = Color.gray;
    [SerializeField] private ChefRecipeBtnManager chefRecipeBtnManager;

    void Start()
    {
        button = GetComponent<Button>();
        icon = GetComponent<Image>();
        UpdateButtonAction();
    }

    // 🔁 If your dishID changes at runtime, you can call this again
    public void UpdateButtonAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnDishClicked);
    }

    private void OnDishClicked()
    {
        Craftingv2.Instance.CurrentDishIdx = dishID;
        chefRecipeBtnManager.OnDishBtnClicked(this);
        Craftingv2.Instance.GetRecipeFromChef();
    }

    public void SetSelected(bool isSelected)
    {
        icon.color = isSelected ? darkColor : normalColor;
    }
}
