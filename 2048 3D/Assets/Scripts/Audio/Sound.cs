using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public TypeSound type;
    public AudioClip clip;
    
    [Range(0, 1)]
    public float volume; 
    //[Range(0.1f, 3f)]
    //public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

}

public enum TypeSound
{
    BACKGROUND, LEVELUP, COMBO, RELEASE, GEM_IMPACT, GAME_OVER
}
