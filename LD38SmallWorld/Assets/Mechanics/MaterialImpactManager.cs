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
			CheckValid();
			return _instance  ;
		}
	}

	private static void CheckValid()
	{
		if (_instance == null)
					throw new InvalidOperationException("Material Manager not initialized!");
	}



	public ImpactEffect GetImpactEffect(Collider collider)
	{
		if (collider == null)
			return null;

		return GetImpactEffect(collider.material);
	}

	public ImpactEffect GetImpactEffect(PhysicMaterial material)
	{
		if (material == null)
		{
			return null;
		}
		return material == null ? null : impactEffects.FirstOrDefault(i => i.material == material);
	}

	void Awake()
	{
		_instance = this;
	}
}

