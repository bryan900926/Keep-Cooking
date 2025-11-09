using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    // Update is called once per frame
    public void SetText(bool isCooking) //
    {
        if (isCooking)
        {
            text.SetText("Cooking ...");
        }
        else
        {
            text.SetText("Cook!");
        }
    }
}
