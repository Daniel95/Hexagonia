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
    [SerializeField] private float pitchMax = 1.5f;
    [SerializeField] private GameObject oneShotAudio;
    
	public void PlayEffect(AudioEffectType _audioType, float _pitch = 1f, float _volume = 1)
    {
        for (int i = 0; i < audioEffects.Count; i++)
        {
            if (audioEffects[i].Effect == _audioType)
            {
                if (audioEffects[i].Clip != null)
                {
                    AudioEffect effect = audioEffects[i];
                    PlayClipAtPoint(effect.Clip, _pitch, effect.Volume);
                }
                break;
            }
        }
    }

    private void PlayClipAtPoint(AudioClip _audioClip, float _pitch = 1f, float _volume = 1)
    {
        GameObject tempGameobject = Instantiate(oneShotAudio, transform.position, Quaternion.identity);
        AudioSource tempSource = tempGameobject.GetComponent<AudioSource>();
        
        tempSource.clip = _audioClip;
        tempSource.pitch = _pitch;
        tempSource.volume = _volume;
        tempSource.Play();

        Destroy(tempGameobject, _audioClip.length);
    }

    private void PlayerDiedSound()
    {
        PlayEffect(AudioEffectType.Death);
    }

    private void HighScore()
    {
        if (XRSettings.enabled)
        {
            if (Progression.VRHighScore > Progression.Instance.Score)
            {
                PlayEffect(AudioEffectType.Highscore);
            }
        }
        else
        {
            if (Progression.NonVRHighScore > Progression.Instance.Score)
            {
                PlayEffect(AudioEffectType.Highscore);
            }
        }
    }

    private void MultiplierMaxCheck(float _multiplier)
    {
        float _pitch = 1 + (pitchMax - 1) * (_multiplier / ResourceValue.Instance.MaxValue);

        PlayEffect(AudioEffectType.MultiplierMax, _pitch);
    }

    private void CoinCollected(int _value)
    {
        PlayEffect(AudioEffectType.Coin);
    }

    private void SwitchedMenuCanvas()
    {
        PlayEffect(AudioEffectType.SwitchedMenuCanvas);
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += PlayerDiedSound;
        PlayerDiedAnimation.CompletedEvent += HighScore;
        Coin.CollectedEvent += CoinCollected;
        ScoreMultiplier.MultiplierIncreasedEvent += MultiplierMaxCheck;
        MainMenuRotator.SwitchedEvent += SwitchedMenuCanvas;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= PlayerDiedSound;
        PlayerDiedAnimation.CompletedEvent -= HighScore;
        Coin.CollectedEvent -= CoinCollected;
        ScoreMultiplier.MultiplierIncreasedEvent -= MultiplierMaxCheck;
        MainMenuRotator.SwitchedEvent -= SwitchedMenuCanvas;
    }
}