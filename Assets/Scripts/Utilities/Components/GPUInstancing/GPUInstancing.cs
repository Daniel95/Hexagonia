using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public InstanceData AddTransform(Transform _transform, Transform _parentTransform = null)
    {
        InstanceData _instanceData = new InstanceData(_transform.position, _transform.localScale, _transform.rotation, _parentTransform);

        if (batchesByName.ContainsKey(_transform.name))
        {
            batchesByName[_transform.name].TransformDatas.Add(_instanceData);
        }
        else
        {
            Batch _batch = new Batch
            {
                TransformDatas = new List<InstanceData>
                {
                    _instanceData,
                },
                Mesh = _transform.GetComponent<MeshFilter>().sharedMesh,
                Material = _transform.GetComponent<MeshRenderer>().sharedMaterial,
            };
            _batch.TransformDatas.Add(_instanceData);

            batchesByName.Add(_transform.name, _batch);
        }
        return _instanceData;
    }

    public void RemoveTransformsByParent(Transform _parentTransform)
    {
        List<string> _keysToRemove = new List<string>();

        foreach (KeyValuePair<string, Batch> _batchByStringPair in batchesByName)
        {
            List<InstanceData> _objectDatas = _batchByStringPair.Value.TransformDatas;

            for (int i = _objectDatas.Count - 1; i >= 0; i--)
            {
                InstanceData _objData = _objectDatas[i];
                if(_objData.Parent != _parentTransform) { continue; }
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

    private void Update()
    {
        if (batchesByName.Count <= 0) { return; }
        RenderBatches();
    }

    private void RenderBatches()
    {
        foreach (KeyValuePair<string, Batch> _batch in batchesByName)
        {
            Graphics.DrawMeshInstanced(_batch.Value.Mesh, 0, _batch.Value.Material, _batch.Value.TransformDatas.Select(a => a.Matrix).ToList());
        }
    }
}