using System.Collections.Generic;
using UnityEngine;


public class BaseSFX : MonoBehaviour
{
    readonly private Dictionary<string, AudioSource> sfxSources = new();

    // Get or create an AudioSource for given SFX
    private AudioSource GetSource(string clipName, bool loop)
    {
        if (!sfxSources.TryGetValue(clipName, out var source))
        {
            source = gameObject.AddComponent<AudioSource>();
            source.loop = loop;
            source.playOnAwake = false;
            sfxSources[clipName] = source;
        }
        return source;
    }

    // Looping SFX (footsteps, charging, etc.)
    public void PlayLoop(string clipName)
    {
        if (AudioManager.Instance.TryGetClip(clipName, out var clip))
        {
            var source = GetSource(clipName, true);
            source.volume = AudioManager.Instance.sfxvolume;
            if (source.clip != clip)
                source.clip = clip;

            if (!source.isPlaying)
                source.Play();
        }
    }

    public void StopLoop(string clipName)
    {
        if (sfxSources.TryGetValue(clipName, out var source))
            source.Stop();
    }

    public void PlayBGM(string clipName)
    {
        if (AudioManager.Instance.TryGetClip(clipName, out var clip))
        {
            var source = GetSource(clipName, true);
            source.volume = AudioManager.Instance.bgmvolume;
            if (source.clip != clip)
                source.clip = clip;

            if (!source.isPlaying)
                source.Play();
        }
    }

    // One-shot SFX (attack, jump, hurt, pickup…)
    public void PlayOneShot(string clipName)
    {
        if (AudioManager.Instance.TryGetClip(clipName, out var clip))
        {
            var source = GetSource(clipName, false);
            source.volume = AudioManager.Instance.sfxvolume;
            source.PlayOneShot(clip);
        }
    }

    public void BuildSourceFromProfile(AudioProfile profile)
    {
        var source = GetSource(profile.clipName, profile.loop);
        if (AudioManager.Instance.TryGetClip(profile.clipName, out var clip))
        {
            source.clip = clip;
            source.loop = profile.loop;
            source.spatialBlend = profile.spatialBlend;
            source.volume = profile.volume;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = profile.minDistance;
            source.maxDistance = profile.maxDistance;

            if (profile.useRandomPitch)
            {
                source.pitch = Random.Range(profile.minPitch, profile.maxPitch);
            }
            else
            {
                source.pitch = 1f;
            }
            sfxSources[profile.clipName] = source;
        }
        else
        {
            Debug.LogWarning($"Audio clip '{profile.clipName}' not found in AudioManager!");
        }
    }
    void OnDisable()
    {
        foreach (var source in sfxSources.Values)
        {
            source.Stop();
        }
    }
}
