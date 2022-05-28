using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AudioConfigurationSO : ScriptableObject
{
    [SerializeField] public AudioDict[] ParticularAudioClips;
    [SerializeField] private AudioClip[] AmbientAudioClips;

    [HideInInspector]
    public List<Transform> ambientAudioLocations;

    public AudioClip GetRandomAmbientAudio()
    {
        return AmbientAudioClips[Random.Range(0, AmbientAudioClips.Length)];
    }

    public AudioClip GetSpecificAudioClip(string nameOfClip)
    {
        return (from pac in ParticularAudioClips where pac.name == nameOfClip select pac.clip).FirstOrDefault();
    }
}

[System.Serializable]
public class AudioDict
{
    public string name;
    public AudioClip clip;
}