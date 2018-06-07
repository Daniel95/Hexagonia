using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Spawns and play's audio effects on custom positions. The script gets the audio from AudioEffect.cs
/// </summary>
public class AudioEffectManager : MonoBehaviour
{
    public static AudioEffectManager Instance{ get { return GetInstance(); } }

    #region Singleton
    private static AudioEffectManager instance;

    private static AudioEffectManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioEffectManager>();
        }
        return instance;
    }
    #endregion
    
    [SerializeField] private List<AudioEffect> audioEffects = new List<AudioEffect>();
    [SerializeField] private float maxMultiplierEffectDelay = 5;
    [SerializeField] private GameObject oneShotAudio;

    private bool maxMultiplierEffectReady = true;
    
    /// <summary>
    /// Plays an audioEffect at a certain worldposition
    /// </summary>
	public void PlayEffect(AudioEffectType _audioType, Vector3 _worldPosition, bool _loop = true)
    {
        for (int i = 0; i < audioEffects.Count; i++)
        {
            if (audioEffects[i].Effect == _audioType)
            {
                if (audioEffects[i].Clip != null)
                {
                    PlayClipAtPoint(audioEffects[i].Clip);
                }
                break;
            }
        }
    }

    private void PlayClipAtPoint(AudioClip _audioClip)
    {
        GameObject tempGameobject = Instantiate(oneShotAudio, transform.position, Quaternion.identity);
        AudioSource tempSource = tempGameobject.GetComponent<AudioSource>();
        
        tempSource.clip = _audioClip;
        tempSource.Play();

        Destroy(tempGameobject, _audioClip.length);
    }

    private void PlayerDiedSound()
    {
        PlayEffect(AudioEffectType.Death, transform.position);
    }

    private void HighScore()
    {
        if (XRSettings.enabled)
        {
            if (Progression.VRHighScore > Progression.Instance.Score)
            {
                PlayEffect(AudioEffectType.Highscore, Camera.main.transform.position);
            }
        }
        else
        {
            if (Progression.NonVRHighScore > Progression.Instance.Score)
            {
                PlayEffect(AudioEffectType.Highscore, Camera.main.transform.position);
            }
        }
    }

    private void MultiplierMaxCheck(int _multiplier)
    {
        if (!maxMultiplierEffectReady) { return; }
        
        if (_multiplier == ResourceValue.Instance.MaxValue)
        {
            PlayEffect(AudioEffectType.MultiplierMax, transform.position);

            maxMultiplierEffectReady = false;
            StartCoroutine(SetSoundDelay(maxMultiplierEffectDelay, maxMultiplierEffectReady));
        }
    }

    private void CoinCollected(int _value)
    {
        PlayEffect(AudioEffectType.Coin, transform.position);
    }

    private void SwitchedMenuCanvas()
    {
        Debug.Log("Called switchmenucanvassound");
        PlayEffect(AudioEffectType.SwitchedMenuCanvas, transform.position);
    }

    private IEnumerator SetSoundDelay(float _delay, bool _switch)
    {
        yield return new WaitForSeconds(_delay);
        _switch = true;
    }

    private void OnEnable()
    {
        Player.DiedEvent += PlayerDiedSound;
        PlayerDiedAnimation.CompletedEvent += HighScore;
        Coin.CollectedEvent += CoinCollected;
        ScoreMultiplier.UpdatedEvent += MultiplierMaxCheck;
        MainMenuRotator.SwitchedEvent += SwitchedMenuCanvas;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= PlayerDiedSound;
        PlayerDiedAnimation.CompletedEvent -= HighScore;
        Coin.CollectedEvent -= CoinCollected;
        ScoreMultiplier.UpdatedEvent -= MultiplierMaxCheck;
        MainMenuRotator.SwitchedEvent -= SwitchedMenuCanvas;
    }
}