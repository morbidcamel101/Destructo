using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

[AddComponentMenu("Small World/Spawner")]
public class Spawner : BehaviorBase
{
	#region Definitions
	public class ActiveObject {

		public ActiveObject(GameObject obj) {
			this.gameObject = obj;
		}

		public GameObject gameObject;

		public bool active;

		public float recycleTime;
	}

	[Serializable]
	public class ObjectCache {
		
		public GameObject prefab;
		public GameObject prototype;
		public int cacheSize = 10;
		public int cacheIndex = 0;
		[HideInInspector]
		public GameObject[] objects;


		public void Initialize()
		{
			CheckValid();
			objects = new GameObject[cacheSize];

			for(var i = 0; i < cacheSize; i++)
			{
				var obj = objects[i] = CreateObject(this); // Instantiate(prefab) as GameObject;
				obj.SetActive(false);
				obj.name = prefab.name + "(" + i.ToString() + ")";
			}

		}

		public GameObject Next()
		{
			CheckValid();
			GameObject obj = null;
			for(int i = 0; i < cacheSize; i++)
			{
				obj = objects[i];

				if (obj.activeSelf)
					continue;

				cacheIndex = (cacheIndex + 1) % cacheSize;
				return obj;
			}
			return obj;
		}

		[Conditional("DEBUG")]
		internal void CheckValid()
		{
			if (!prefab)
				throw new InvalidProgramException("The prefab for the cache entry is not specified.");
		}

	}
	#endregion

	#region Behavior
	public ObjectCache[] caches;
	public GameObject cacheRoot;
	public Dictionary<GameObject, ActiveObject> activeObjects;

	public static Spawner spawner;
	// Use this for initialization
	void Start ()
	{
		spawner = this;

		if (!cacheRoot)
			cacheRoot = gameObject;

		int amount = 0;
		for(int i = 0; i < caches.Length; i++)
		{
			var cache = caches[i];
			cache.Initialize();

			for(int c = 0; c < cache.cacheSize; c++)
			{
				// Organize objects
				Organize(cache.objects[c], cache);
			}
			amount += cache.cacheSize;
		}

		activeObjects = new Dictionary<GameObject, ActiveObject>(amount);


	
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		var hasScheduled = false;

		foreach(var kv in activeObjects)
		{
			var obj = kv.Value;
			if (obj.recycleTime == 0)
				continue;

			hasScheduled = true;
			if (Time.time < obj.recycleTime)
				continue;

			obj.recycleTime = 0;
			obj.active = false;
			kv.Key.SetActive(false);
		}
		if (!hasScheduled)
			enabled = false;
	
	}

	public void Organize(GameObject obj, ObjectCache cache)
	{
		CheckValid();
		cache.CheckValid();

		if (cache.prototype)
		{
			obj.transform.SetParent(cache.prototype.transform);
			return;
		}

		// Make a transform under the cache root (Recycle Bin)
		var dest = cacheRoot.transform.FindChild(cache.prefab.name);
		if (dest == null)
		{
			var newObj = new GameObject(cache.prefab.name);
			newObj.transform.SetParent(cacheRoot.transform, false);
			dest = newObj.transform;
			UnityEngine.Debug.Log(string.Format("{0} -> {1}",cache.prefab.name, dest.name));
		}
		obj.transform.SetParent(dest, false);
	}

	public ObjectCache GetCacheForPrefab(GameObject prefab)
	{
		for(int i = 0; i < caches.Length; i++)
		{
			if (caches[i].prefab == prefab)
				return caches[i];
		}
		return null;

	}

	[Conditional("DEBUG")]
	private void CheckValid()
	{
		if (!cacheRoot)
			throw new InvalidProgramException("The prefab is not assigned for the spawner entry");
	}
	#endregion

	#region Static Methods

	private static GameObject CreateObject (ObjectCache cache)
	{
		if (!spawner.cacheRoot || !cache.prefab)
			throw new InvalidProgramException("Cache root or cache prefab is not configured");
		var res = MonoBehaviour.Instantiate (cache.prefab);
		if (cache.prototype != null)
		{
			res.transform.Align(cache.prototype);
			res.transform.SetParent(cache.prototype.transform, false);
		}
		else
		{	
			res.transform.SetParent(spawner.cacheRoot.transform);
		}
		spawner.Organize (res, cache);
		return res;
	}

	private static GameObject CreateObject (GameObject prefab)
	{
		
		return MonoBehaviour.Instantiate(prefab, spawner.cacheRoot.transform);
	}

	public static GameObject Spawn(Prototype prototype, bool cacheOnly = false, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
	{
		return Spawn(prototype.prefab, cacheOnly, position, rotation);
	}

	public static GameObject Spawn(GameObject prefab, bool cacheOnly = false, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
	{
		if (spawner == null)
		{
			UnityEngine.Debug.LogError("The spawner was not initialized!");
			return null;
		}

		spawner.Ensure(prefab, "prefab");

		var cache = spawner.GetCacheForPrefab(prefab);
		if (cache == null) 
			return CreateObject (prefab);
		
		// Get available  object in the cache
		var obj = cache.Next();
		if (cacheOnly && obj == null)
			return null;

		if (obj == null) 
		{
			spawner.Log("Cache unavailable for {0} -- increase cache size??", prefab);
			return CreateObject(cache); // Cache Leak??
		}

		if (cache.prototype != null && position == Vector3.zero)
		{
			position = cache.prototype.transform.position;
			rotation = cache.prototype.transform.rotation;
		}

		obj.transform.position = position;
		obj.transform.rotation = rotation;
		

		obj.SetActive(true);

		if (!spawner.activeObjects.ContainsKey(obj))
			spawner.activeObjects.Add(obj, new ActiveObject(obj)  { active = true });

		return obj;
	}



	public static void Recycle(UnityEngine.Object objectToDestroy, float delay = 0f)
	{
		if (objectToDestroy == null)
			return;

		if (objectToDestroy is Component)
			objectToDestroy = ((Component)objectToDestroy).gameObject;

		var destroy = (GameObject)objectToDestroy;

		// Is it in the cache and being tracked?
		if (spawner.activeObjects.ContainsKey(destroy))
		{
			if (delay <= 0)
			{
				destroy.SetActive(false);
				spawner.activeObjects[destroy].active = false;
			}
			else
			{
				spawner.activeObjects[destroy].recycleTime = Time.time + delay;
				spawner.enabled = true; // Activate and wait!
			}
		}
		else
		{
			if (delay <= 0f)
				GameObject.DestroyImmediate(destroy);
			else
				GameObject.Destroy(destroy, delay);
		}
	}
	#endregion
}

