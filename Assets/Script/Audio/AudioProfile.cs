using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Profile")]
public class AudioProfile : ScriptableObject
{
    public string clipName;
    public bool loop = false;

    [Range(0, 1)]
    public float volume = 1f;

    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float spatialBlend = 1f;

    [Header("Random Pitch")]
    public bool useRandomPitch = false;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
}
