using UnityEngine;
using System.Collections;


[AddComponentMenu("Small World/Spawn Point")]
public class SpawnPoint : BehaviorBase 
{
	public float delay = 5;
	public float checkDelay = 5f;
	internal float resumeTime;
	public Vector3 offset = new Vector3(1f, 0.2f);

	//private bool isReady = false;

	public bool IsReady
	{
		get {
			return Time.time > resumeTime;
		}
	}

	public void Notify()
	{
		resumeTime = Time.time + delay;

	}


	void Awake()
	{
		var boxCol = GetComponent<BoxCollider>();
		if (boxCol != null)
			Destroy(boxCol);

		var sphere = gameObject.AddComponent<SphereCollider>();
		sphere.radius = 0.5f;
	}


	public void Deploy()
	{
		var rigid = GetComponent<Rigidbody>();
		if (rigid)
			Destroy(rigid);

		var sphere = GetComponent<SphereCollider>();
		if (sphere)
			Destroy(sphere);
	}

	/*void Start()
	{
		isReady = false;
	}

	void Update()
	{
		if (Time.time < resumeTime)
			return;

		resumeTime = Time.time + checkDelay;

		Ray ray = new Ray(transform.position, -transform.up);
		if (!(isReady = Physics.Raycast(ray, 10f)))
		{
			Spawner.Recycle(gameObject,5f);
		}
	}

	void OnTriggerStay(Collider other)
	{
		
		isReady = other is TerrainCollider;;
	}*/
}
