using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Small World/Terrain Manager")]
[RequireComponent(typeof(TerrainCollider))]
[RequireComponent(typeof(Terrain))]
public  sealed class TerrainManager : BehaviorBase {

	// Just needs to be there!
	//Terrain terrain;

	void Awake()
	{
		//terrain = GetComponent<Terrain>();	
	}

	void Update()
	{
		
	}

	public void GenerateTreeImpact()
	{
		/* TODO
		for(int i = 0; i < terrain.terrainData.treeInstances.Length; i++)
		{
			var tree = terrain.terrainData.treeInstances[i];
			tree.

		}*/
	}

}
