using UnityEngine;
using System.Collections;


[AddComponentMenu("Small World/Spawn Point")]
public class SpawnPoint : BehaviorBase 
{
	
	public float delay = 5;

	internal float resumeTime;

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
}
