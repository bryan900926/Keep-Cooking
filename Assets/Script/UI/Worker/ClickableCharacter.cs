using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class ClickableCharacter<T> : MonoBehaviour, IPointerClickHandler where T : MonoBehaviour
{
    protected T stateManager;

    [SerializeField] private Level levelSystem;

    private void Start()
    {
        stateManager = GetComponentInParent<T>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{typeof(T).Name} clicked");

        if (stateManager != null)
        {
            levelSystem.LevelUp();
        }
        else
        {
            Debug.LogWarning($"{typeof(T).Name} not found in parent!");
        }
    }
}
