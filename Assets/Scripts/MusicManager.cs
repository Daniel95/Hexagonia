using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public static MusicManager Instance
    {
        get
        {
            return GetInstance();
        }
    }
    private static MusicManager instance;

    private AudioSource source;
    private AudioClip currentClip;

    [Space(5)]

    [SerializeField] private List<Song> songList = new List<Song>();

    public void SwitchSong(bool _fade = true, bool _useIntro = false)
    {
        if (songList.Count == 0)
            return;


    }
    
    private static MusicManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<MusicManager>();
        }
        return instance;
    }
}
