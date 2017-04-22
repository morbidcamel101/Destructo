using UnityEngine;
using System.Collections;

public static class Extensions 
{
	public static void Align(this Transform source, Transform target)
	{
		source.position = target.position;
		source.rotation = target.rotation;
	}

	public static void Align(this Transform source, GameObject target)
	{
		Align(source, target.transform);
	}
}

