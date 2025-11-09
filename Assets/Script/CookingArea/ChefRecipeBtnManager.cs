using UnityEngine;

public class ChefRecipeBtnManager : MonoBehaviour
{
    [SerializeField] private ChefRecipeBtn[] Btns;


    public void OnDishBtnClicked(ChefRecipeBtn clickedBtn)
    {
        foreach (var btn in Btns)
        {
            // If it's the clicked one, darken it; otherwise, reset
            bool isSelected = (btn == clickedBtn);
            btn.SetSelected(isSelected);
        }
    }

}
