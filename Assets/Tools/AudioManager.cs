using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Script containing all sounds used in the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Tooltip("List of sounds which can be used in the game")]
    [SerializeField]
    public List<Sound> Sounds;
    
    public static AudioManager instance;
    
    private static readonly Object syncRoot = new Object();
    
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
                        if (instance == null)
                            Debug.LogError("SingletoneBase<T>: Could not found GameObject of type " + nameof(AudioManager));
                    }
                }
            }
            return instance;
        }
        set { }
    }
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            for (int i = 0; i < Sounds.Count; i++)
            {
                Sounds[i].Source = gameObject.AddComponent<AudioSource>();
                Sounds[i].Source.clip = Sounds[i].Clip;
                Sounds[i].Source.volume = Sounds[i].Volume;
                Sounds[i].Source.pitch = Sounds[i].Pitch;
                Sounds[i].Source.loop = Sounds[i].Loop;
            }
        }
    }

    public void Play(string name)
    {

        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == name);
        if (s == null)
            return;
        s.Source.Play();
    }
    
    public void Play(Sound name)
    {
        Sound s = Array.Find(Sounds.ToArray(), sound => sound.Name == name.Name);
        if (s == null)
            return;
        s.Source.Play();
    }
}
