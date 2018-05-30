using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and play's audio effects on custom positions. The script gets the audio from AudioEffect.cs
/// </summary>

public class AudioEffectManager : MonoBehaviour
{
	[SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private List<AudioEffect> audioEffects = new List<AudioEffect>();

	private void PlayAudio(AudioEffectType _audioType, Transform _transform)
    {
        AudioSource _audioSource = audioSourcePrefab;
        Instantiate(_audioSource.gameObject, _transform.position, Quaternion.identity);

        _audioSource.clip = null;
        for (int i = 0; i < audioEffects.Count; i++)
        {
            if (audioEffects[i].Effect == _audioType)
            {
                _audioSource.clip = audioEffects[i].Clip;
                break;
            }
        }
        if (_audioSource.clip == null)
        {
            Debug.LogError("Could not find a matching sound clip");
            return;
        }

        _audioSource.loop = false;
        _audioSource.Play();
    }
}