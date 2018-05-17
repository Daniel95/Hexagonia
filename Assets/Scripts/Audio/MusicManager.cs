using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays and switches the current playing song
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance
    {
        get
        {
            return GetInstance();
        }
    }
    #region SingleTon
    private static MusicManager instance;

    private static MusicManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<MusicManager>();
        }
        return instance;
    }
    #endregion


    private AudioSource source;
    private AudioClip currentClip;

    private bool switching = false;
    [SerializeField] private List<Song> songList = new List<Song>();
    
    [Space(5)]

    [SerializeField] private float fadeTime = .5f;
    
    [Range(0,1)]
    [SerializeField] private float maxVolume = .5f;

    //Remove input, is for debug
    private void Update()
    {
        if (source != null)
        {
            source.volume = maxVolume;
            if (source.isPlaying == false)
                SwitchSong();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SwitchSong();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = maxVolume;
    }
    
    /// <summary>
    /// Switches to a random song in the songlist
    /// </summary>
    /// <param name="_fade">Depending on this the song fades or switches instantly</param>
    public void SwitchSong(bool _fade = true)
    {
        if (songList.Count == 0 || switching)
            return;

        Song _randomSong = RandomSong();
        switching = true;

        GivePriority();
        
        if (_fade)
        {
            StartCoroutine(FadeToNewSong(_randomSong));
        }
        else
        {
            source.clip = _randomSong.clip;
            source.Play();
            switching = false;
        }
        return;
    }

    private IEnumerator FadeToNewSong(Song _song)
    {
        StartCoroutine(FadeOut());

        yield return new WaitForSeconds(fadeTime / 2f);

        source.clip = _song.clip;
        source.volume = 0;
        source.Play();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        source.volume = maxVolume;
        float _timePassed = 0;

        while (_timePassed < fadeTime/2f)
        {
            source.volume = maxVolume - (maxVolume * (_timePassed / (fadeTime / 2f)));

            _timePassed += Time.deltaTime;
            yield return null;
        }
        source.volume = 0;
    }
    private IEnumerator FadeIn()
    {
        source.volume = 0;
        float _timePassed = 0;

        while (_timePassed < fadeTime / 2f)
        {
            source.volume = (maxVolume * (_timePassed / (fadeTime / 2f)));

            _timePassed += Time.deltaTime;
            yield return null;
        }
        source.volume = maxVolume;
        switching = false;
    }

    /// <summary>
    /// Loops trough all the songs in the songlist, trying to find one with a high (low number) priority
    /// </summary>
    /// <returns>A random song to use</returns>
    private Song RandomSong()
    {
        Song _randomSong = null;
        int _count = 0;
        
        while (_randomSong == null)
        {
            Song _potentialSong = songList[Random.Range(0, songList.Count)];

            if (_potentialSong.priority <= _count)
            {
                _randomSong = _potentialSong;
                _potentialSong.priority += songList.Count;
            }
            _count += 1;

            if (_count >= 1000)
                break;
        }
        return _randomSong;
    }

    /// <summary>
    /// Makes it more likely for all songs to be picked again 
    /// </summary>
    private void GivePriority()
    {
        foreach (Song _song in songList)
        {
            if (_song.priority > 0)
            {
                _song.priority -= 1;
            }
        }
    }

}