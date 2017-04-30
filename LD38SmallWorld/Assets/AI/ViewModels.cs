using UnityEngine;
using System.Collections;

// Not for designer
// My solution to a non physics imapact 
public class HitInfo 
{
	public RaycastHit hit;

	public ITarget target;

	public bool locked { get { return target is DynamicTarget; } }

}