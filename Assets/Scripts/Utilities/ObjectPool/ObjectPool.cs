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

    /// <summary>
    /// The pooled objects currently available.
    /// Indexed by the index of the objectPrefabs
    /// </summary>
    /// 
    private const string OBJECT_POOL_ENTRIES_PATH = "ChunkPoolEntries/";

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

    void OnEnable()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{
		//Loop through the object prefabs and make a new list for each one.
		//We do this because the pool can only support prefabs set to it in the editor,
		//so we can assume the lists of pooled objects are in the same order as object prefabs in the array

		for (int i = 0; i < Entries.Count; i++)
		{
			ObjectPoolEntry objectPrefab = Entries[i];

			//create the repository
			objectPrefab.pool = new List<GameObject>(objectPrefab.Count);

			//fill it                      
			for (int n = 0; n < objectPrefab.Count; n++)
			{
				GameObject newObj = (GameObject)Instantiate(objectPrefab.Prefab);
				newObj.name = objectPrefab.Prefab.name;
				PoolObject(newObj);
			}
		}

	    if (PoolingInitialisationCompletedEvent != null)
	    {
	        PoolingInitialisationCompletedEvent();
        }
	}

	/// <summary>
	/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
	/// then null will be returned.
	/// </summary>
	/// <returns>
	/// The object for type.
	/// </returns>
	/// <param name='objectType'>
	/// Object type.
	/// </param>
	/// <param name='onlyPooled'>
	/// If true, it will only return an object if there is one currently pooled.
	/// </param>
	public GameObject GetObjectForType(string objectType, bool onlyPooled)
	{

		for (int i = 0; i < Entries.Count; i++)
		{
		    ObjectPoolEntry objectPoolEntry = Entries[i];
            GameObject prefab = objectPoolEntry.Prefab;

			if (prefab.name != objectType)
				continue;

			if (Entries[i].objectsInPool > 0)
			{

				GameObject pooledObject = Entries[i].pool[--Entries[i].objectsInPool];
				pooledObject.transform.parent = null;
				pooledObject.SetActive(true);

				return pooledObject;
			}
            else if (!onlyPooled)
			{
				GameObject obj = Instantiate(objectPoolEntry.Prefab);
				//obj.name = obj.name+"_not_pooled";
                //string objName = obj.name;
                obj.name = obj.name.Substring(0, obj.name.Length - 7);
			    objectPoolEntry.pool.Add(obj);

                return obj;
			}
		}

		//If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
		return null;
	}

	/// <summary>
	/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
	/// </summary>
	/// <param name='obj'>
	/// Object to be pooled.
	/// </param>
	public void PoolObject(GameObject obj)
	{
		for (int i = 0; i < Entries.Count; i++)
		{
			if (Entries[i].Prefab.name != obj.name)
				continue;

			obj.SetActive(false);
			obj.transform.parent = transform;
            /*
			if (obj.GetComponent<Rigidbody>() != null) {
				obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
            */
			Entries[i].pool[Entries[i].objectsInPool++] = obj;
			return;
		}
		Destroy(obj);
	}

}