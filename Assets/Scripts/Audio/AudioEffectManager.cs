using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and play's audio effects on custom positions. The script gets the audio from AudioEffect.cs.
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

    public float PitchMax { get { return pitchMax; } }
    
    [SerializeField] private List<AudioEffect> audioEffects = new List<AudioEffect>();
    [SerializeField] private float pitchMax = 1.5f;
    [SerializeField] private GameObject oneShotAudio;

    /// <summary>
    /// Plays the audio effect linked to the AudioEffectType. Can also adjust the pitch and volume.
    /// </summary>
    /// <param name="_audioType"></param>
    /// <param name="_pitch"></param>
    /// <param name="_volume"></param>
    public void PlayEffect(AudioEffectType _audioType, float _pitch = 1f, float _volume = 1)
    {
        for (int i = 0; i < audioEffects.Count; i++)
        {
            if (audioEffects[i].Effect == _audioType)
            {
                if (audioEffects[i].Clip != null)
                {
                    AudioEffect _effect = audioEffects[i];
                    PlayClipAtPoint(_effect.Clip, _pitch, _effect.Volume);
                }
                break;
            }
        }
    }

    private void PlayClipAtPoint(AudioClip _audioClip, float _pitch = 1f, float _volume = 1)
    {
        GameObject _tempGameobject = Instantiate(oneShotAudio, transform.position, Quaternion.identity);
        AudioSource _tempSource = _tempGameobject.GetComponent<AudioSource>();
        
        _tempSource.clip = _audioClip;
        _tempSource.pitch = _pitch;
        _tempSource.volume = _volume;
        _tempSource.Play();

        Destroy(_tempGameobject, _audioClip.length);
    }
}