using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjData
{
    public Transform parent;
    public Vector3 localPosition;
    public Vector3 scale;
    public Quaternion rot;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(GetPos, rot, scale);
        }
    }

    public ObjData(Vector3 localPosition, Vector3 scale, Quaternion rot, Transform parent = null)
    {
        this.localPosition = localPosition;
        this.scale = scale;
        this.rot = rot;
        this.parent = parent;
    }

    private Vector3 GetPos
    {
        get
        {
            Vector3 _position = localPosition;
            if (parent != null)
            {
                _position = parent.TransformPoint(_position);
            }
            return _position;
        }
    }
}

public class Batch
{
    public Mesh objMesh;
    public Material objMat;

    public List<ObjData> ObjDatas;
}

public class GPUInstancing : MonoBehaviour
{
    public static GPUInstancing Instance { get { return GetInstance(); } }

    #region Singleton
    private static GPUInstancing instance;

    private static GPUInstancing GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GPUInstancing>();
        }
        return instance;
    }
    #endregion

    private Dictionary<string, Batch> batchesByName = new Dictionary<string, Batch>();

    void Update()
    {
        if (batchesByName.Count > 0)
        {
            RenderBatches();
        }
    }

    public ObjData AddTransform(Transform _transform, Transform _parentTransform = null)
    {
        ObjData _objData = new ObjData(_transform.position, _transform.localScale, _transform.rotation, _parentTransform);

        if (batchesByName.ContainsKey(_transform.name))
        {
            batchesByName[_transform.name].ObjDatas.Add(_objData);
        }
        else
        {
            Batch _batch = new Batch
            {
                ObjDatas = new List<ObjData>
                {
                    _objData,
                },
                objMesh = _transform.GetComponent<MeshFilter>().sharedMesh,
                objMat = _transform.gameObject.GetComponent<MeshRenderer>().sharedMaterial,
            };
            _batch.ObjDatas.Add(_objData);

            batchesByName.Add(_transform.name, _batch);
        }
        return _objData;
    }

    public void RemoveTransformsByParent(Transform _parentTransform)
    {
        List<string> _keysToRemove = new List<string>();

        foreach (KeyValuePair<string, Batch> _batchByStringPair in batchesByName)
        {
            List<ObjData> _objectDatas = _batchByStringPair.Value.ObjDatas;

            for (int i = _objectDatas.Count - 1; i >= 0; i--)
            {
                ObjData _objData = _objectDatas[i];
                if(_objData.parent != _parentTransform) { continue; }
                _objectDatas.RemoveAt(i);
            }

            if (_objectDatas.Count <= 0)
            {
                _keysToRemove.Add(_batchByStringPair.Key);
            }
        }

        foreach (string _key in _keysToRemove)
        {
            batchesByName.Remove(_key);
        }
    }

    private void RenderBatches()
    {
        foreach (KeyValuePair<string, Batch> _batch in batchesByName)
        {
            Graphics.DrawMeshInstanced(_batch.Value.objMesh, 0, _batch.Value.objMat, _batch.Value.ObjDatas.Select(a => a.matrix).ToList());
        }
    }
}