using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundID
{
    Shoot, BulletExplosion, BuildingExplosion, GameEnd
}

[Serializable]
public struct SoundSrc
{
    [SerializeField]
    AudioClip soundFile;
    public AudioClip SoundFile
    {
        get
        {
            return soundFile;
        }
    }

    [SerializeField]
    SoundID soundID;
    public SoundID ID
    {
        get
        {
            return soundID;
        }
    }
}

[CreateAssetMenu]
public class AudioStorage : ScriptableObject
{
    [SerializeField]
    SoundSrc[] soundSrcs;

    Dictionary<SoundID, AudioClip> dicSounds = new Dictionary<SoundID, AudioClip>();

    void GenerateDictionary()
    {
        for(int i = 0; i<soundSrcs.Length;i++)
        {
            dicSounds.Add(soundSrcs[i].ID, soundSrcs[i].SoundFile);
        }
    }

    public AudioClip Get(SoundID ID)
    {
        if(dicSounds.Count == 0)
        {
            GenerateDictionary();
        }
        return dicSounds[ID];
    }
}