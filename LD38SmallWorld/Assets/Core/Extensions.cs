using UnityEngine;
using System.Collections;
using System.Diagnostics;

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


	[Conditional("DEBUG")]
	public static void Assert(this Object context, bool condition, string message)
	{
		if (condition)
			UnityEngine.Debug.Assert(condition, message, context);
	}
}

