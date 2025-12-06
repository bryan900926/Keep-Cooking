using UnityEngine;

public class CustomerSFX : BaseSFX
{
    [SerializeField] private AudioProfile[] profiles;

    void Start()
    {
        foreach (var profile in profiles)
        {
            BuildSourceFromProfile(profile);
        }
    }

    public void PlayEating()
    {
        PlayLoop("CustomerEating");
    }

    public void StopEating()
    {
        StopLoop("CustomerEating");
    }

    public void PaidMoney()
    {
        PlayOneShot("money", 0.5f);
    }

    public void FallInHole()
    {
        PlayOneShot("Whoosh");
    }
}