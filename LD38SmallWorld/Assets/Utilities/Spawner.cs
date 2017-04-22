using UnityEngine;
using System.Collections;
using System;

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
		public int cacheSize = 10;
		public int cacheIndex = 0;
		public GameObject root;
		[HideInInspector]
		public GameObject[] objects;


		public void Initialize()
		{
			objects = new GameObject[cacheSize];

			for(var i = 0; i < cacheSize; i++)
			{
				var obj = objects[i];
				obj = Instantiate(prefab) as GameObject;
				obj.SetActive(true);
				obj.name = prefab.name + "(" + i.ToString() + ")";
			}
		}

		public GameObject Next()
		{
			GameObject obj = null;
			for(int i = 0; i < cacheSize; i++)
			{
				obj = objects[i];

				if (obj == null || !obj.activeSelf || obj)
					break;

				cacheIndex = (cacheIndex + 1) % cacheSize;
			}
			if (obj == null)
					return null;

			cacheIndex = (cacheIndex + 1) % cacheSize;
			return obj;
		}

	}
	#endregion

	#region Behavior

	public static Spawner spawner;
	// Use this for initialization
	void Awake ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	#endregion
}

