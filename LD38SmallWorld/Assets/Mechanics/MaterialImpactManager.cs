using UnityEngine;
using System.Linq;
using System.Collections;
using System;

[AddComponentMenu("Small World/Material Impact Manager")]
public class MaterialImpactManager : BehaviorBase
{
	[Serializable]
	public class ImpactEffect 
	{
		public string id;
		public PhysicMaterial material;
		public Prototype effect;
	}

	public ImpactEffect[] impactEffects;

	private static MaterialImpactManager _instance;
	public static MaterialImpactManager Instance
	{
		get {
			return _instance ?? (_instance = new MaterialImpactManager());
		}
	}

	public ImpactEffect this[PhysicMaterial material]
	{
		get {
			if (material == null)
			{
				Debug.LogError("The material cannot be null - management");
				return null;
			}
			return impactEffects.FirstOrDefault(i => i.material == material);
		}
	}
}

