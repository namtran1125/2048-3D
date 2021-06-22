using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    public Sound[] combos;
    public Sound[] Gem_impact;
    void Awake()
    {
        Instance = this;
        foreach (Sound s in combos)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        foreach (Sound s in Gem_impact)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        PlaySoudManager(0);
    }
    public void PlaySoudManager(int _s)
    {
        sounds[_s].source.Play();
    }
    public void ComboSound(int _combo)
    {
        combos[_combo].source.Play();
    }
    public void GemImpactSound()
    {
        //int r = UnityEngine.Random.Range(0, Gem_impact.Length);
        //Gem_impact[r].source.Play();
        Gem_impact[3].source.Play();
    }
    //public void PlaySound(TypeSound type)
    //{
    //    Sound s = Array.Find(sounds, s => s.type == type);
    //    sounds.source.Play();
    //    sounds[0].source.Play();
    //}

}
