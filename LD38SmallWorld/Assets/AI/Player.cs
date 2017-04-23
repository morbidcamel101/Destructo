using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}

		if (Input.GetButtonUp("Fire1"))
		{
			Fire();
		}
	}

	void FixedUpdate()
	{
		// http://answers.unity3d.com/questions/13022/aiming-gun-at-cursor.html
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
			SetTarget(hit.point);
		else
			SetTarget(ray.GetPoint(100));
	}



	protected override void OnDeath ()
	{
		// GAME OVER!!
		// TODO - Handle death

		this.GetComponent<FirstPersonController>().enabled = false;
		Log("YOU ARE DEAD! GAME OVER!");

		throw new NotImplementedException();

	}

	protected override void OnCriticalHealth ()
	{
		Log("YOU HAVE CRITICAL HEALTH!");
		// TODO - Show Blood overlay
		
	}

	protected override void OnLowHealth ()
	{
		Log("HEALTH IS LOW!!");
	}

	protected override void OnImpact (Bullet bullet)
	{
		// Ouch
	}
}


