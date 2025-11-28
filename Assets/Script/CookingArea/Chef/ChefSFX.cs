using UnityEngine;

public class ChefSFX : BaseSFX
{
    [SerializeField] private AudioProfile[] profiles;

    void Start()
    {
        foreach (var profile in profiles)
        {
            BuildSourceFromProfile(profile);
        }
    }

    public void PlayCooking()
    {
        Debug.Log("Playing ChefCooking SFX");
        PlayLoop("ChefCooking");
    }
    public void StopCooking()
    {
        StopLoop("ChefCooking");
    }
}