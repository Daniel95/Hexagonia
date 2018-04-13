using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObjectIdRef))]
public class ObjectIdRefDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.propertyType != SerializedPropertyType.String) {
            EditorGUI.LabelField(position, label.text, "Use ObjectIdRef only with a string field.");
            return;
        }

        EditorGUILayout.BeginHorizontal();
        property.stringValue = EditorGUILayout.TextField(property.displayName, property.stringValue);

        if (GUILayout.Button("Highlight", GUILayout.MaxWidth(70))) {
            ObjectId obj = FindObject(property);
            if (obj) {
                EditorGUIUtility.PingObject(obj.gameObject);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private ObjectId FindObject(SerializedProperty property) {
        ObjectId[] objects = Object.FindObjectsOfType<ObjectId>();
        foreach (ObjectId obj in objects) {
            if (obj.Id != property.stringValue) { continue; }
            return obj;
        }

        Debug.LogWarning("Object not found in scene!");

        return null;
    }

}