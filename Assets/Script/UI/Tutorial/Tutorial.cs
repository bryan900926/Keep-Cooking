using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject chefTutorialObject;
    [SerializeField] private GameObject customerTutorialObject;
    [SerializeField] private GameObject waiterTutorialObject;
    [SerializeField] private GameObject repuationTutorialObject;
    [SerializeField] private GameObject beerTutorialObject;

    readonly private Dictionary<TutorialPage, string> tutorialPagesText = new()
    {
        { TutorialPage.BACKGROUND, backGroundStory },
        { TutorialPage.CHEF, chef },
        { TutorialPage.CUSTOMER, customer },
        { TutorialPage.WAITER, waiter },
        { TutorialPage.REPUATION, repuation },
        { TutorialPage.BEER, beer }
    };
    readonly Dictionary<TutorialPage, GameObject> tutorialPagesObjects = new()
    {
    };
    private Coroutine activeTextCoroutine;

    private int curPageIdx = 0;

    readonly private TutorialPage[] pages = new TutorialPage[]
    {
        TutorialPage.BACKGROUND,
        TutorialPage.CHEF,
        TutorialPage.CUSTOMER,
        TutorialPage.WAITER,
        TutorialPage.REPUATION,
        TutorialPage.BEER
    };
    private const string backGroundStory = "You are now transported to another world and have become a tavern owner. You must manage both front- and back-of-house staff and handle all kinds of issues in the restaurant… Good luck!";
    private const string chef = "When chef forget the food recipe, you would see a sweat drop on the chef's head. You can click on the chef to correct what recipe they are trying to cook (corrent recipe is at keyboard M). When the food stock is low, the yellow warning sign will appear above the chef's head. Make sure to restock the ingredients in time with keyboard I!";
    private const string customer = "Customers will come in and order food. If the food is too expensive (you can change the price with keyboard M) or if they have to wait too long, your repuatation would decrease. Keep an eye on their patience which is shown above their head! They would tip you more for better service and cheaper food.";
    private const string waiter = "Waiters would deliver food from counter to customerers. They would be slowed down if they step on the oil spill on the ground. Make sure to clean them up with keyboard R!";

    private const string repuation = "Higher reputation would bring in more generous customers. Keep your repuatation high by providing good service and delicious food!";

    private const string beer = "Get close to the beer machine and Press R to make the beer which can help increase customer patience!";
    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        tutorialPagesObjects[TutorialPage.CHEF] = chefTutorialObject;
        tutorialPagesObjects[TutorialPage.BACKGROUND] = null;
        tutorialPagesObjects[TutorialPage.CUSTOMER] = customerTutorialObject;
        tutorialPagesObjects[TutorialPage.WAITER] = waiterTutorialObject;
        tutorialPagesObjects[TutorialPage.REPUATION] = repuationTutorialObject;
        tutorialPagesObjects[TutorialPage.BEER] = beerTutorialObject;
        ShowMessage(tutorialPagesText[pages[curPageIdx]]);
    }
    private void ShowTutorialGameObject(TutorialPage page)
    {
        foreach (var obj in tutorialPagesObjects.Values)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        if (tutorialPagesObjects.ContainsKey(page) && tutorialPagesObjects[page] != null)
        {
            tutorialPagesObjects[page].SetActive(true);
        }
    }
    private void ShowMessage(string message)
    {
        if (activeTextCoroutine != null)
            StopCoroutine(activeTextCoroutine);

        gameObject?.SetActive(true);
        activeTextCoroutine = StartCoroutine(FadeInOut(message));
        ShowTutorialGameObject(pages[curPageIdx]);
    }

    private IEnumerator FadeInOut(string message)
    {
        messageText.text = "";
        // Type out words progressively
        string[] words = message.Split(' ');
        foreach (string word in words)
        {
            messageText.text += (messageText.text.Length > 0 ? " " : "") + word;
            yield return new WaitForSeconds(2f / words.Length);
        }

    }

    private void OnBackButtonClicked()
    {
        if (curPageIdx > 0)
        {
            curPageIdx--;
        }
        else
        {
            curPageIdx = pages.Length - 1;
        }
        UISFX.Instance.PlayChangePage();
        ShowMessage(tutorialPagesText[pages[curPageIdx]]);
    }

    private void OnNextButtonClicked()
    {
        if (curPageIdx < pages.Length - 1)
        {
            curPageIdx++;
        }
        else
        {
            curPageIdx = 0;
        }
        UISFX.Instance.PlayChangePage();
        ShowMessage(tutorialPagesText[pages[curPageIdx]]);
    }
    public void ClearAllObJects()
    {
        foreach (var obj in tutorialPagesObjects.Values)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}

public enum TutorialPage
{
    BACKGROUND,
    CHEF,
    CUSTOMER,
    WAITER,
    REPUATION,
    BEER
}
