using UnityEngine;
public class PlayerSFX : BaseSFX
{
    [SerializeField] private AudioProfile[] profiles;

    void Start()
    {
        foreach (var profile in profiles)
        {
            BuildSourceFromProfile(profile);
        }
    }
    public void PlayFootstep()
    {
        PlayLoop("PlayerWalking");
    }

    public void StopFootstep()
    {
        StopLoop("PlayerWalking");
    }
}
