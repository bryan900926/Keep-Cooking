using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class UImanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UImanager Instance;
    [SerializeField] private GameObject[] uiElements;
    private Dictionary<GameObject, CanvasGroup> canvasgroup = new Dictionary<GameObject, CanvasGroup>();
    private CanvasGroup Currentcg;
    private CanvasGroup Priorcg;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        for (int i = 0; i < uiElements.Length; i++) {
            canvasgroup[uiElements[i]] = uiElements[i].GetComponent<CanvasGroup>();
            if (i >= 1)
            {
                HideUI(uiElements[i].GetComponent<CanvasGroup>());
            }
        }
        Currentcg = uiElements[0].GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnUI()
    {
        ClickShowUI(Priorcg.gameObject.name);
        ClickHideUI();
    }

    public void HideUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ClickHideUI()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current.GetComponentInParent<CanvasGroup>() == null) return;
        Currentcg = current.GetComponentInParent<CanvasGroup>();
        HideUI(Currentcg);
        Priorcg = Currentcg;
    }

    public void ClickShowUI(string name)
    {
        GameObject current = GameObject.Find(name);
        Currentcg = current.GetComponentInParent<CanvasGroup>();
        ShowUI(Currentcg);
    }


}
