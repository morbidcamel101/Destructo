using UnityEngine;
using System.Collections;

[AddComponentMenu("Small World/Trash")]
public class Trash : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Destroy(this.gameObject);
	
	}
}

