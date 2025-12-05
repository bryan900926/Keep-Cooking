using System.Collections;
using UnityEngine;

public class UISFX : BaseSFX
{
    public static UISFX Instance { get; private set; }
    [SerializeField] private AudioProfile[] profiles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        yield return null; // wait 1 frame so AudioManager is ready

        foreach (var profile in profiles)
            BuildSourceFromProfile(profile);

        PlayBGM("BGM");
    }

    public void PlayButtonClick()
    {
        PlayOneShot("ButtonClick");
    }

    public void PlayChangePage()
    {
        PlayOneShot("ChangePage");
    }

    public void PlayPlaceItem()
    {
        PlayOneShot("PlaceItem");
    }

    public void PlayPurchaseItem()
    {
        PlayOneShot("Purchasing");
    }

    public void PlayChangePrice()
    {
        PlayOneShot("Pencil");
    }

    public void PlayGameOver()
    {
        PlayOneShot("GameOver");
    }

    public void PlayLevelUp()
    {
        PlayOneShot("LevelUp");
    }
    public void PlayMoney()
    {
        PlayOneShot("money");
    }

    public void PlayWrong()
    {
        PlayOneShot("Wrong");
    }

    public void PlaySuccess()
    {
        PlayOneShot("Success");
    }

    public void Addreputation()
    {
        PlayOneShot("plus", 1.5f);
    }

    public void Minusreputation()
    {
        PlayOneShot("minus", 1.5f);
    }

}