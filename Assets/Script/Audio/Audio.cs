using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private AudioClip current;
    
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bgmsource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSourcePool.Add(source);
        }

    }

    private void Start()
    {
        Addressables.LoadAssetsAsync<AudioClip>("Audio", clip =>
        {
            audioClips[clip.name] = clip;
            //Debug.Log($"¸ü¤J¤ù¬q¡G{clip.name}");
        }, true);
    }

    public void OnBGMVolumeChange(float value)
    {
        int volume = Mathf.RoundToInt(100 * value);
        bgmsource.volume = value;
        bgmvalue.text = volume.ToString();
    }

    public void OnSFXVolumeChange(float value)
    {
        int volume = Mathf.RoundToInt(100 * value);
        sfxvolume = value;
        sfxvalue.text = volume.ToString();
    }



    public void PlayBGM(string name)
    {   
        if (audioClips.TryGetValue(name, out var clip)){   
            if (current == clip){
                return;
            }
            else{
                bgmsource.clip = clip;
                bgmsource.volume = bgmvolume;
                bgmsource.Play();
                bgmsource.loop = true;
            }
        }
        current = clip;
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource source = GetAvailableSource();
        if (source == null){
            return;
        }

        source.clip = clip;
        source.volume = sfxvolume;
        source.Play();
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
