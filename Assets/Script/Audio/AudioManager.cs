using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AudioManager Instance;

    public AudioSource bgmsource;

    public TextMeshProUGUI bgmvalue;
    public TextMeshProUGUI sfxvalue;

    public int poolSize = 20;

    public float sfxvolume = 0.5f;
    public float bgmvolume = 0.5f;

    private List<AudioSource> audioSourcePool = new();

    private AudioClip current;

    Dictionary<string, AudioClip> audioClips = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        bgmsource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSourcePool.Add(source);
        }

        Addressables.LoadAssetsAsync<AudioClip>("Audio", clip =>
        {
            audioClips[clip.name] = clip;
        }, true);

    }

    public void OnBGMVolumeChange(float value)
    {
        int volume = Mathf.RoundToInt(100 * value);
        bgmvolume = value/5;
        bgmvalue.text = volume.ToString();
        UISFX.Instance.PlayBGM("BGM");
    }

    public void OnSFXVolumeChange(float value)
    {
        int volume = Mathf.RoundToInt(100 * value);
        sfxvolume = value/5;
        sfxvalue.text = volume.ToString();
    }



    public void PlayBGM(string name)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            if (current == clip)
            {
                return;
            }
            else
            {
                bgmsource.clip = clip;
                bgmsource.volume = bgmvolume;
                bgmsource.Play();
                bgmsource.loop = true;
            }
        }
        current = clip;
    }

    public void PlaySFX(string clipName, Transform spawnTransform, float volume)
    {
        if (!audioClips.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
            return;
        }
        AudioSource source = GetAvailableSource();
        if (source == null)
        {
            return;
        }

        source.clip = clip;
        source.volume = volume;
        source.transform.position = spawnTransform.position;
        source.Play();
    }

    public bool TryGetClip(string name, out AudioClip clip)
    {
        return audioClips.TryGetValue(name, out clip);
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }
}
