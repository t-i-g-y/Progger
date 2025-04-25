using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (SoundEffectGroup group in soundEffectGroups)
        {
            soundDictionary[group.name] = group.clips;
        }
    }

    public AudioClip GetRandomClip(string groupName)
    {
        if (soundDictionary.ContainsKey(groupName))
        {
            List<AudioClip> clips = soundDictionary[groupName];
            if (clips.Count > 0)
            {
                return clips[UnityEngine.Random.Range(0, clips.Count)];
            }
        }

        return null;
    }
}

[System.Serializable]
public struct SoundEffectGroup
{
    public string name;
    public List<AudioClip> clips;
}
