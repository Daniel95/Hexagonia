using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton script to get the component PostEffectMask in the scene
/// </summary>

[RequireComponent(typeof(PostEffectMask))]
public class PostProcessingLink : MonoBehaviour {

    public static PostProcessingLink Instance { get { return GetInstance(); } }

    public PostEffectMask PostEffectMask()
    {
        if (postEffectMask == null)
        {
            postEffectMask = GetComponent<PostEffectMask>();
        }
        return postEffectMask;
    }

    #region Singleton
    private static PostProcessingLink instance;

    private static PostProcessingLink GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PostProcessingLink>();
        }
        return instance;
    }
    #endregion

    private PostEffectMask postEffectMask;
}