using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{
	public int score = 0;
	internal HitInfo aim = new HitInfo();



	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}

		if (Input.GetButtonUp("Fire1"))
		{
			StopFire();
		}
	}

	void FixedUpdate()
	{
		// http://answers.unity3d.com/questions/13022/aiming-gun-at-cursor.html
		var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)); // Camera.main.ScreenPointToRay(Input.mousePosition); 



		if (Physics.Raycast(ray, out aim.hit))
			SetTarget(aim.hit.point);
		else
			SetTarget(ray.GetPoint(10000));
	}

	public override void SetTarget (Vector3 target)
	{
		base.SetTarget (target);
		aim.target = target;
	}



	protected override void OnDeath ()
	{
		// GAME OVER!!
		// TODO - Handle death

		//this.GetComponent<FirstPersonController>().enabled = false;
		Log("YOU ARE DEAD! GAME OVER!");

		// Respawn until game over;


		
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
		bullet.Hit(); // Yes ouch

	}

	protected override void CanImpact (Bullet bullet)
	{
		if (bullet.sender is Thug)
			bullet.Hit();
	}

}


