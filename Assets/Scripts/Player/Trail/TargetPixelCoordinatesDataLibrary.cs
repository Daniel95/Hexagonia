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

    public List<Vector2> GetTargetPixelCoordinates(Sprite sprite, TargetPixelCoordinatesLabel targetPixelCoordinatesLabel)
    {
        TargetPixelCoordinatesData _targetPixelCoordinatesData = targetPixelCoordinatesDatas.Find(x => x.Sprite == sprite && x.Label == targetPixelCoordinatesLabel);
        if(_targetPixelCoordinatesData == null)
        {
            Debug.LogError("targetPixelCoordinates with sprite " + sprite.name + " and label " + targetPixelCoordinatesLabel + " does not exist");
            return null;
        }

        return _targetPixelCoordinatesData.TargetPixelCoordinates;
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