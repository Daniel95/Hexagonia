using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorHelper
{

    [MenuItem("Tools/Generate Scene List", priority = 800)]
    public static void GenerateSceneList()
    {
        SceneListCheck.Generate();
    }

    [MenuItem("Tools/Delete PlayerPrefs", priority = 800)]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Go to Persistant Data path folder", priority = 800)]
    public static void OpenPersistantDataPathFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("Tools/Go to Data path folder", priority = 800)]
    public static void OpenDataPathFolder()
    {
        EditorUtility.RevealInFinder(Application.dataPath);
    }

    [MenuItem("Tools/Remove Debug Objects", priority = 800)]
    public static void RemoveDebugObjects()
    {
        DebugHelper.RemoveAllDebugPositions();
    }

    [MenuItem("Tools/Check for duplicate ObjectId's", priority = 800)]
    public static void CheckDuplicateIds()
    {
        List<ObjectId> objectIds = new List<ObjectId>(Object.FindObjectsOfType<ObjectId>());

        List<string> doubles = new List<string>();

        foreach (ObjectId id in objectIds)
        {
            if (doubles.Contains(id.Id)) { continue; }

            List<ObjectId> doubleObjectIds = objectIds.FindAll(x => x.Id == id.Id && x.gameObject != id.gameObject);

            if (doubleObjectIds != null && doubleObjectIds.Count > 0)
            {
                doubles.Add(id.Id);

                Debug.Log("FOUND DOUBLES FOR " + id.name, id.gameObject);

                foreach (ObjectId doubleId in doubleObjectIds)
                {
                    Debug.Log(doubleId.name, doubleId.gameObject);
                }
            }

        }

        if (doubles.Count == 0)
        {
            Debug.Log("NO DOUBLES FOUND");
        }
    }

    [MenuItem("Tools/Regenerate ObjectIds in scene", priority = 800)]
    public static void RegenerateObjectIds()
    {

        if (EditorUtility.DisplayDialog("  Warning: Object Ids are delicate beeings.", "This will generate new object id's for all open scenes! Ask a programmer before you press 'Ok'.", "Ok", "Nevermind"))
        {
            List<ObjectId> objectIds = new List<ObjectId>(Object.FindObjectsOfType<ObjectId>());
            foreach (ObjectId id in objectIds)
            {
                id.GenerateId();
            }
        }

    }


}
