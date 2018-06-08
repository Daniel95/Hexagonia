using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolbag;

/// <summary>
/// Repository of commonly used prefabs.
/// </summary>

public class ObjectPool : MonoBehaviour {

	public static ObjectPool Instance { get { return GetInstance(); } }

    public static Action PoolingInitialisationCompletedEvent;

    #region Singleton
    private static ObjectPool instance;

    private static ObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ObjectPool>();
        }
        return instance;
    }
    #endregion

    #region member
    /// <summary>
    /// Member class for a prefab entered into the object pool
    /// </summary>
    [Serializable]
	public class ObjectPoolEntry {
		/// <summary>
		/// the object to pre instantiate
		/// </summary>
		[SerializeField]
		public GameObject Prefab;

		/// <summary>
		/// quantity of object to pre-instantiate
		/// </summary>
		[SerializeField]
		public int Count;

		[HideInInspector]
		public List<GameObject> pool;

		[HideInInspector]
		public int objectsInPool = 0;
	}
	#endregion

	/// <summary>
	/// The object prefabs which the pool can handle
	/// by The amount of objects of each type to buffer.
	/// </summary>
	[Reorderable] public List<ObjectPoolEntry> Entries;

    private Dictionary<string, ObjectPoolEntry> entriesByName = new Dictionary<string, ObjectPoolEntry>();

    /// <summary>
    /// The pooled objects currently available.
    /// Indexed by the index of the objectPrefabs
    /// </summary>
    /// 
    private const string OBJECT_POOL_ENTRIES_PATH = "ChunkPoolEntries/";
    private const int CLONE_NAME_LENGTH = 7;

    /// <summary>
    /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectName'>
    /// Object type.
    /// </param>
    /// <param name='onlyPooled'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetObjectForType(string objectName, bool onlyPooled)
    {
        if (!entriesByName.ContainsKey(objectName))
        {
            Debug.Log("cant find " + objectName);
            return null;
        }

        ObjectPoolEntry objectPoolEntry = entriesByName[objectName];

        if (objectPoolEntry.objectsInPool > 0)
        {
            GameObject pooledObject = objectPoolEntry.pool[--objectPoolEntry.objectsInPool];
            pooledObject.transform.parent = null;
            pooledObject.SetActive(true);

            return pooledObject;
        }
        else if (!onlyPooled)
        {
            GameObject obj = Instantiate(objectPoolEntry.Prefab);

            obj.name = obj.name.Substring(0, obj.name.Length - CLONE_NAME_LENGTH);
            objectPoolEntry.pool.Add(obj);

            return obj;
        }

        return null;
    }

    /// <summary>
    /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
    /// </summary>
    /// <param name='obj'>
    /// Object to be pooled.
    /// </param>
    public void PoolObject(GameObject obj, bool resetRigidbody = false)
    {
        if (!entriesByName.ContainsKey(obj.name))
        {
            Destroy(obj);
            return;
        }

        ObjectPoolEntry objectPoolEntry = entriesByName[obj.name];

        obj.SetActive(false);
        obj.transform.parent = transform;
        if (resetRigidbody)
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        objectPoolEntry.pool[objectPoolEntry.objectsInPool++] = obj;
    }

    [ContextMenu("UpdateObjectPoolEntry")]
    private void UpdateObjectPoolEntry()
    {
        Entries.Clear();

        List<GameObject> _prefabs = Resources.LoadAll<GameObject>(OBJECT_POOL_ENTRIES_PATH).ToList();

        foreach (GameObject _prefab in _prefabs)
        {
            ObjectPoolEntry objectPoolEntry = new ObjectPoolEntry
            {
                Prefab = _prefab,
            };

            Entries.Add(objectPoolEntry);
        }
    }

    // Use this for initialization
    private void Start()
	{
        //Loop through the object prefabs and make a new list for each one.
        //We do this because the pool can only support prefabs set to it in the editor,
        //so we can assume the lists of pooled objects are in the same order as object prefabs in the array

        for (int i = 0; i < Entries.Count; i++)
		{
			ObjectPoolEntry objectPoolEntry = Entries[i];

            entriesByName.Add(objectPoolEntry.Prefab.name, objectPoolEntry);

			//create the repository
			objectPoolEntry.pool = new List<GameObject>(objectPoolEntry.Count);

			//fill it                      
			for (int n = 0; n < objectPoolEntry.Count; n++)
			{
				GameObject newObj = (GameObject)Instantiate(objectPoolEntry.Prefab, transform);
				newObj.name = objectPoolEntry.Prefab.name;
                newObj.SetActive(false);
			}
		}

	    if (PoolingInitialisationCompletedEvent != null)
	    {
	        PoolingInitialisationCompletedEvent();
        }
	}

}