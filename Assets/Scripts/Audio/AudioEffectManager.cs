using System.Collections.Generic;
using UnityEngine;

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

    public float PitchMax { get { return pitchMax; } }
    
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
}