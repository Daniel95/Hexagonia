using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the audio, plays and switches the current playing song and between scenes.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get { return GetInstance(); } }

    #region Singleton
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

    [Range(0, 1)] [SerializeField] private float maxVolume = .5f;
    [SerializeField] private SongsByScenesPair[] songsByScenePairs;
	[SerializeField] private float fadeTime = 0.5f;

    private AudioSource source;
    private AudioClip currentClip;
    private List<Song> songs = new List<Song>();
    private Coroutine delayCoroutine;

    /// <summary>
    /// Starts playing a random song for the current scene.
    /// </summary>
    public void StartMusic()
    {
        if (!source.isPlaying)
        {
            SwitchSong();
        }
    }
    
    /// <summary>
    /// Switches to a random song in the songlist for the current scene.
    /// </summary>
    public void SwitchSong()
    {
        if (songs.Count == 0) { return; }
        
        Song _randomSong = RandomSong();

        if (_randomSong == null) { return; }

        GivePriority();

        StartCoroutine(FadeToNewSong(_randomSong));

        float _delay = _randomSong.Clip.length;

        DelayCoroutineCheck();

        delayCoroutine = CoroutineHelper.DelayTime(_delay, () => SwitchSong());
    }

    private void DelayCoroutineCheck()
    {
        if (delayCoroutine != null)
        {
            CoroutineHelper.Stop(delayCoroutine);
            delayCoroutine = null;
        }
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = maxVolume;
    }

    private IEnumerator FadeToNewSong(Song _song)
    {
        if (source.isPlaying)
        {
            StartCoroutine(FadeOut());

            yield return new WaitForSeconds(fadeTime / 2f);
        }
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
    }

    private Song RandomSong()
    {
        Song _randomSong = null;
        int _count = 0;
        
        while (_randomSong == null)
        {
            Song _potentialSong = songs[UnityEngine.Random.Range(0, songs.Count)];

            if (_potentialSong.Priority <= _count)
            {
                _randomSong = _potentialSong;
                _potentialSong.Priority += songs.Count;
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
        foreach (Song _song in songs)
        {
            if (_song.Priority > 0)
            {
                _song.Priority -= 1;
            }
        }
    }

    private void SceneSwitch(Scenes? _oldScene, Scenes _newScene)
    {
        foreach (SongsByScenesPair _list in songsByScenePairs)
        {
            if (_list.Scene == _newScene)
            {
                songs = _list.Songs;
            }
        }

        if (_oldScene == _newScene) { return; }
        if (_newScene == Scenes.Intro) { return; }

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