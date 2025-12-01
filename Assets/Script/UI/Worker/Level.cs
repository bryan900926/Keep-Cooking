using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private int level = 1;

    public int LevelValue => level;

    [SerializeField] private LevelUpHint levelUpHint;

    [SerializeField] private Holding holdingSystem;

    public void LevelUp()
    {
        levelUpHint.ShowLevelUpHint();
        level++;
        textMeshPro.text = "Lv: " + level;
        holdingSystem.AddCapacity();
    }

}
