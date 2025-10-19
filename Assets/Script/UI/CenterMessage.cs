using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class CenterMessage : MonoBehaviour
{
    public static CenterMessage Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeSpeed = 2f;

    private Coroutine activeCoroutine;

    public readonly static string MENU_UPDATED = "Please press the menu button to see updates.";
    public readonly static string CHEF_LEAVE = "Chef has left your kitchen. Please assign a new chef to continue cooking.";

    public readonly static string SUCCESSFUL_COOK = "Chef is cooking your dish!";

    public readonly static string FOOD_ROTTEN = "Please clean up the rotten food left by the chef.";


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        canvasGroup.alpha = 0f;
        messageText.text = "";
    }

    public void ShowMessage(String message)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        gameObject.SetActive(true);
        activeCoroutine = StartCoroutine(FadeInOut(message));
    }

    private IEnumerator FadeInOut(string message)
    {
        messageText.text = "";
        canvasGroup.alpha = 0f;

        // Fade in
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Type out words progressively
        string[] words = message.Split(' ');
        foreach (string word in words)
        {
            messageText.text += (messageText.text.Length > 0 ? " " : "") + word;
            yield return new WaitForSeconds(displayDuration / words.Length);
        }

        // Hold the message briefly
        yield return new WaitForSeconds(displayDuration * 0.5f);

        // Fade out
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        messageText.text = "";
    }
}
