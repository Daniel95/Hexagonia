using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectId //: MonoBehaviour {

    public string Id { get { return id; } }

    private static readonly Dictionary<string, ObjectId> instances = new Dictionary<string, ObjectId>();

    [SerializeField] private string id;

    [ContextMenu("GenerateId Id")]
    public string GenerateId() {
        id = Guid.NewGuid().ToString();
        return id;
    }

    public static GameObject Find(string objectId) {
        if (!instances.ContainsKey(objectId)) { return null; }
        return instances[objectId].gameObject;
    }

    private void Awake() {
        instances.Add(id, this);
    }

    private void OnDestroy() {
        instances.Remove(id);
    }

    private void Reset() {
        GenerateId();
    }

}
