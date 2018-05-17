using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class TargetPixelCoordinatesDataLibrary : MonoBehaviour
{

    public static TargetPixelCoordinatesDataLibrary Instance { get { return GetInstance(); } }

    private static TargetPixelCoordinatesDataLibrary instance;

    private static TargetPixelCoordinatesDataLibrary GetInstance()
    {
        if(instance == null)
        {
            instance = Resources.Load<TargetPixelCoordinatesDataLibrary>("TargetPixelCoordinatesDataLibrary");
        }
        return instance;
    }

    [Reorderable] [SerializeField] private List<TargetPixelCoordinatesData> targetPixelCoordinatesDatas;

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
    }

}