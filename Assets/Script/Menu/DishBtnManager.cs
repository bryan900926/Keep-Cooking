using UnityEngine;

public class DishBtnManager : MonoBehaviour
{
    [SerializeField] private DishBtn[] dishBtns;
    public void OnDishBtnClicked(DishBtn clickedBtn)
    {
        foreach (var btn in dishBtns)
        {
            // If it's the clicked one, darken it; otherwise, reset
            bool isSelected = (btn == clickedBtn);
            btn.SetSelected(isSelected);
        }
    }
}
