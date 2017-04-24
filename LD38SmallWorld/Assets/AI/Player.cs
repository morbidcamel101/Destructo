using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{
	public int score = 0;
	internal HitInfo aim = new HitInfo();

	public Transform head;
	public Transform zoomPoint;
	public Transform cameraMount;
	public float focusTime = 1;
	private float zoomTime;
	private float unzoomTime;

	void Awake()
	{
		Ensure(head);
		Ensure(zoomPoint);
		Ensure(cameraMount);
		this.Assert(focusTime > 0, "Focus time needs to be > 0");
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Fire("uzi");
		}

		if (Input.GetButtonUp("Fire1"))
		{
			StopFire("uzi");
		}

		if (Input.GetButtonDown("Fire2"))
		{
			Fire("rocket");
		}

		if (Input.GetButtonUp("Fire2"))
		{
			Fire("rocket");
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


		// Check the gun status
		var gun = GetGun("rocket");  // YES!!
		if (gun != null)
		{
			UpdateGunBehavior(gun);
		}
			
	}

	private void UpdateGunBehavior(Gun gun)
	{
		switch(gun.state)
		{
			case Gun.State.Fire: 
			case Gun.State.Load:
			case Gun.State.Loading:
			case Gun.State.Loaded:
				PerformZoom();
				break;
			default:
				if (gun.fire)
				{
					PerformZoom();
					break;
				}

				PerformUnzoom();
				break;
		}
	}

	private void PerformZoom()
	{
		if (zoomTime > 0)
		{
			var t =  Mathf.Clamp01( (zoomTime - Time.time) / focusTime );
			cameraMount.transform.position = Vector3.Slerp(head.position, zoomPoint.position, t);
		}
		unzoomTime = Time.time + focusTime;
	}

	private void PerformUnzoom()
	{
		if (unzoomTime > 0)
		{
			var t1 = Mathf.Clamp01( (unzoomTime - Time.time) / focusTime );
			cameraMount.transform.position = Vector3.Slerp(zoomPoint.position, head.position, t1);
		}
		zoomTime = Time.time + focusTime;
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


