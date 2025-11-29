using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [Header("Hotkey Setting")]
    public KeysForUI hotkey;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (Toggle.Instance != null)
        {
            Toggle.Instance.RegisterPanel(hotkey, canvasGroup);
        }
    }
    private void OnDisable()
    {
        if (Toggle.Instance != null)
        {
            Toggle.Instance.UnregisterPanel(hotkey);
        }
    }
}
