using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [Header("Hotkey Setting")]
    public Key hotkey;  // 例如 Key.V、Key.Escape

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // 自動註冊到 Toggle 管理器
        if (Toggle.Instance != null)
        {
            Toggle.Instance.RegisterPanel(hotkey, canvasGroup);
        }
    }
}
