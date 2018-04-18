using System.Collections.Generic;
using UnityEngine;

public class AudioEffectManager : MonoBehaviour
{
    public static AudioEffectManager Instance { get { return GetInstance(); } }
    private static AudioEffectManager instance;

    [SerializeField] private AudioSource prefab;

    [SerializeField] private List<AudioEffect> audioEffects = new List<AudioEffect>();

    /// <summary>
    /// Play's an audio effect at a certain position
    /// </summary>
    void PlayAudio(AudioEffectType _audioType, Transform _transform)
    {
        AudioSource _audioSource = prefab;
        Instantiate(_audioSource.gameObject, _transform.position, Quaternion.identity);

        _audioSource.clip = null;
        for (int i = 0; i < audioEffects.Count; i++)
        {
            if (audioEffects[i].Effect == _audioType)
            {
                _audioSource.clip = audioEffects[i].clip;
                break;
            }
        }
        if (_audioSource.clip == null)
            Debug.LogError("Could not find a matching sound clip");

        _audioSource.loop = false;
        _audioSource.Play();
    }

    private static AudioEffectManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioEffectManager>();
        }
        return instance;
    }
}