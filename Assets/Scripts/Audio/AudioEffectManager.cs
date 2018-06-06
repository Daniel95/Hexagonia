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
    
    [SerializeField] private List<AudioEffect> audioEffects = new List<AudioEffect>();

    [Space(5)]


    [SerializeField] private GameObject oneShotAudio;

    /// <summary>
    /// Plays an audioEffect at a certain worldposition
    /// </summary>
	public void PlayEffect(AudioEffectType _audioType, Vector3 _worldPosition)
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
}