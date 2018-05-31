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
    }

    private Vector3 GetPos
    {
        get
        {
            Vector3 position = localPosition;
            if (parent != null)
            {
                position = parent.TransformPoint(position);
            }
            return position;
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

    public ObjData AddObj(Transform _transform)
    {
        ObjData _objData = new ObjData(_transform.position, _transform.localScale, _transform.rotation, transform.parent);
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
                objMat = _transform.gameObject.GetComponent<MeshRenderer>().sharedMaterial
            };
            
            batchesByName.Add(_transform.name, _batch);
        }

        return _objData;
    }

    public void RemoveObj(Transform _transform)
    {
        if (batchesByName.ContainsKey(_transform.name))
        {
            batchesByName.Remove(_transform.name);
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