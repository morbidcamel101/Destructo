using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;

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
	public static void Assert(this UnityEngine.Object context, bool condition, string message)
	{
		UnityEngine.Debug.Assert(condition, message, context);
	}

	public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
	{
		foreach(var item in col)
		{
			action(item);
		}
	}
}

