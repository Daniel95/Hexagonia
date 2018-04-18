using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private static MusicManager instance;

    private AudioSource source;
    private AudioClip currentClip;

    private bool switching = false;
    [SerializeField] private List<Song> songList = new List<Song>();
    
    [Space(5)]

    [SerializeField] private float fadeTime = .5f;

    //Remove, is for debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SwitchSong();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    public void SwitchSong(bool _fade = true)
    {
        if (songList.Count == 0 || switching)
            return;

        switching = true;
        UpPriority();
        Song _randomSong = RandomSong();
        
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
        source.volume = 1;
        float _timePassed = 0;
        while (_timePassed < fadeTime/2f)
        {
            source.volume = 1 - (_timePassed / (fadeTime / 2f));

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
            source.volume = (_timePassed / (fadeTime / 2f));

            _timePassed += Time.deltaTime;
            yield return null;
        }
        source.volume = 1;
        switching = false;
    }

    private Song RandomSong()
    {
        Song _randomSong = null;
        int count = 0;
        
        while (_randomSong == null)
        {
            Song _potentialSong = songList[Random.Range(0, songList.Count)];

            if (_potentialSong.priority <= count)
            {
                _randomSong = _potentialSong;
                _potentialSong.priority += songList.Count;
            }
            count += 1;

            if (count >= 1000)
                break;
        }
        return _randomSong;
    }

    private void UpPriority()
    {
        foreach (Song _song in songList)
        {
            if (_song.priority > 0)
            {
                _song.priority -= 1;
            }
        }
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
