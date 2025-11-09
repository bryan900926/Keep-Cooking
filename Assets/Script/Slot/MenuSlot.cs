using UnityEngine;
using UnityEngine.UI;
using System.Linq; // ✅ add this line
public class MenuSlot : MonoBehaviour
{
    private Image image;

    public Image Image => image;

    private void Awake()
    {
        if (image == null)
        {
            // Find the first Image that's not on this object
            image = GetComponentsInChildren<Image>(includeInactive: true)
                        .FirstOrDefault(img => img.gameObject != gameObject);

            if (image == null)
                Debug.LogWarning($"{name}: No child Image found!");
        }
    }

}
