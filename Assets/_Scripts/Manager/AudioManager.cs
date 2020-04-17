using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioStorage soundStorage;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void PlaySound(SoundID ID)
    {
        AudioSource.PlayClipAtPoint(soundStorage.Get(ID), Vector3.zero);
    }
}
