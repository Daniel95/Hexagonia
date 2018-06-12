using System;
using System.Collections.Generic;
using UnityEngine;

public class InstanceId //: MonoBehaviour {

    public string Id { get {
            if (!GeneratedId) { GenerateId(); }
            return id;
    } }

    private bool GeneratedId { get { return !string.IsNullOrEmpty(id); } }

    private static readonly Dictionary<string, InstanceId> instances = new Dictionary<string, InstanceId>();

    private string id;

    private string GenerateId() {
        id = Guid.NewGuid().ToString();
        return id;
    }

    public static GameObject Find(string objectId) {
        if (!instances.ContainsKey(objectId)) { return null; }
        return instances[objectId].gameObject;
    }

    private void Awake() {
        if(!GeneratedId) { GenerateId(); }
        instances.Add(id, this);
    }

    private void OnDestroy() {
        instances.Remove(id);
    }

}
