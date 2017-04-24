using UnityEngine;
using System.Collections;

[AddComponentMenu("Small World/No Go Zone")]
public class NoGoZone : BehaviorBase
{

	void OnTriggerEnter(Collider other)
	{
		var character = other.GetComponent<CharacterBase>();
		if (!character)
			return;

		character.Health.Impact(100000000f);
	}
}

