using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private int level = 1;

    [SerializeField] private LevelUpHint levelUpHint;

    public void LevelUp()
    {
        levelUpHint.ShowLevelUpHint();
        level++;
        textMeshPro.text = "Lv: " + level;
    }

}
