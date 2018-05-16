using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour {

    [SerializeField] private Color targetColor;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        List<Vector3> _worldPositionsOfColor = spriteRenderer.GetWorldPositionsOfColor(targetColor);

        Vector3 _combinedTopRight = new Vector3();
        int _topRightCount = 0;

        Vector3 _combinedTopLeft = new Vector3();
        int _topLeftCount = 0;

        Vector3 _combinedBottomRight = new Vector3();
        int _bottomRightCount = 0;

        Vector3 _combinedBottomLeft = new Vector3();
        int _bottomLeftCount = 0;

        Vector3 _tranformPosition = transform.position;

        foreach (Vector3 _position in _worldPositionsOfColor) {
            bool top = _position.y > _tranformPosition.y;
            bool right = _position.x > _tranformPosition.x;

            if(top && right) 
            {
                _combinedTopRight += _position;
                _topRightCount++;
            } 
            else if(top && !right) 
            {
                _combinedTopLeft += _position;
                _topLeftCount++;
            }
            else if(!top && right) 
            {
                _combinedBottomRight += _position;
                _bottomRightCount++;

            }
            else 
            {
                _combinedBottomLeft += _position;
                _bottomLeftCount++;
            }
        }

        Vector3 averageTopRight = _combinedTopRight / _topRightCount;
        Vector3 averageTopLeft = _combinedTopLeft / _topLeftCount;
        //Vector3 averageBottomRight = _combinedBottomRight / _bottomRightCount;
        //Vector3 averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;

        Debug.Log("averageTopRight " + averageTopRight);
        Debug.Log("averageTopLeft " + averageTopLeft);


        //Debug.Log("averageBottomRight " + averageBottomRight);
        //Debug.Log("averageBottomLeft " + averageBottomLeft);

        DebugHelper.SetDebugPosition(averageTopRight, "averageTopRight");
        DebugHelper.SetDebugPosition(averageTopLeft, "averageTopLeft");
        //DebugHelper.SetDebugPosition(averageBottomRight, "averageBottomRight");
        //DebugHelper.SetDebugPosition(averageBottomLeft, "averageBottomLeft");
    }

}
