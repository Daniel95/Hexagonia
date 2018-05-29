using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles the audio, plays and switches the current playing song and between scenes.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float maxVolume = .5f;
	[SerializeField] private Scenes defaultSongList;
    [SerializeField] private Songlist[] songlists;
	[SerializeField] private float fadeTime = 0.5f;

    private AudioSource source;
    private AudioClip currentClip;
    private bool switching = false;
    private List<Song> currentSongList = new List<Song>();
    private Coroutine delayCoroutine;

    /// <summary>
    /// Switches to a random song in the songlist
    /// </summary>
    /// <param name="_fade">Depending on _fade the song fades or switches instantly</param>
    public void SwitchSong(bool _fade = true)
    {
        if (currentSongList.Count == 0) { return; }

        if (switching)
        {
            StopAllCoroutines();
            if (delayCoroutine != null)
            {
                CoroutineHelper.Stop(delayCoroutine);
            }
        }

        Song _randomSong = RandomSong();
        switching = true;

        GivePriority();
        
        if (_fade)
        {
            StartCoroutine(FadeToNewSong(_randomSong));
        }
        else
        {
            source.clip = _randomSong.Clip;
            source.Play();
            switching = false;
        }

        float _delay = _randomSong.Clip.length + 0.1f;
        delayCoroutine = CoroutineHelper.DelayTime(_delay, () => SwitchSong());

        return;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = maxVolume;
        SceneSwitch(Scenes.Default, defaultSongList);
    }

    private IEnumerator FadeToNewSong(Song _song)
    {
        StartCoroutine(FadeOut());

        yield return new WaitForSeconds(fadeTime / 2f);

        source.clip = _song.Clip;
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

    private Song RandomSong()
    {
        Song _randomSong = null;
        int _count = 0;
        
        while (_randomSong == null)
        {
            Song _potentialSong = currentSongList[UnityEngine.Random.Range(0, currentSongList.Count)];

            if (_potentialSong.Priority <= _count)
            {
                _randomSong = _potentialSong;
                _potentialSong.Priority += currentSongList.Count;
            }
            _count += 1;

            if (_count >= 1000)
            {
                break;
            }
        }
        return _randomSong;
    }

    private void GivePriority()
    {
        foreach (Song _song in currentSongList)
        {
            if (_song.Priority > 0)
            {
                _song.Priority -= 1;
            }
        }
    }

    private void SceneSwitch(Scenes _oldScene, Scenes _newScene)
    {
        if (_oldScene == _newScene) { return; }

        foreach (Songlist _list in songlists)
        {
            if (_list.Scene == _newScene)
            {
                currentSongList = _list.SongList;
            }
        }
        SwitchSong();
    }

    private void OnEnable()
    {
        SceneLoader.SceneSwitchCompletedEvent += SceneSwitch;
    }

    private void OnDisable()
    {
        SceneLoader.SceneSwitchCompletedEvent -= SceneSwitch;
    }
}

[Serializable]
internal class Songlist
{
    #pragma warning disable CS0649,
    public Scenes Scene;
    public List<Song> SongList;
    #pragma warning restore CS0649,
}