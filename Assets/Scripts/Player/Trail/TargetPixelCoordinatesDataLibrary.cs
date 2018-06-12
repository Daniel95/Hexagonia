using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Saves the target pixels coordinates of a within a certain color range in sprites.
/// </summary>
public class TargetPixelCoordinatesDataLibrary //: MonoBehaviour
{
    public static TargetPixelCoordinatesDataLibrary Instance { get { return GetInstance(); } }

    #region Singleton
    private static TargetPixelCoordinatesDataLibrary instance;

    private static TargetPixelCoordinatesDataLibrary GetInstance()
    {
        if(instance == null)
        {
            instance = Resources.Load<TargetPixelCoordinatesDataLibrary>("TargetPixelCoordinatesDataLibrary");
        }
        return instance;
    }
    #endregion

    [Reorderable] [SerializeField] private List<TargetPixelCoordinatesData> targetPixelCoordinatesDatas;

    /// <summary>
    /// Retrieve the pixelcoordinates of pixels in the sprite parameter with TargetPixelCoordinatesLabel. 
    /// Get the pixel coordinates of a color in the sprite parameter. 
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="_targetPixelCoordinatesLabel"></param>
    /// <returns></returns>
    public List<Vector2> GetTargetPixelCoordinates(Sprite _sprite, TargetPixelCoordinatesLabel _targetPixelCoordinatesLabel)
    {
        TargetPixelCoordinatesData _targetPixelCoordinatesData = targetPixelCoordinatesDatas.Find(x => x.Label == _targetPixelCoordinatesLabel);
        if (_targetPixelCoordinatesData == null)
        {
            Debug.LogError("TargetPixelCoordinatesData with label " + _targetPixelCoordinatesLabel + " does not exist");
            return null;
        }

        List<Vector2> _targetPixelCoordinates = _targetPixelCoordinatesData.GetTargetPixelCoordinates(_sprite);
        return _targetPixelCoordinates;
    }

    [ContextMenu("Update")]
    private void UpdateTargetPixels()
    {
        foreach (TargetPixelCoordinatesData _targetPixelCoordinatesData in targetPixelCoordinatesDatas)
        {
            _targetPixelCoordinatesData.UpdateTargetPixelCoordinates();
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}