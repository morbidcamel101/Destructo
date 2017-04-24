using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{
	public int score = 0;
	internal HitInfo aim = new HitInfo();

	public Transform head;
	public Transform zoomPoint;
	public Transform cameraMount;
	public float smoothing = 5f;
	public float zoom = 20f;
	public float normal = 60f;
	public bool isZooming;

	void Awake()
	{
		Ensure(head);
		Ensure(zoomPoint);
		Ensure(cameraMount);
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
			SetTarget(ray.GetPoint(1000));


		// Check the gun status
		UpdateGuns();
			
	}

	private void UpdateGuns()
	{
		if (Input.GetButtonDown("Fire3"))
		{
			isZooming = true;
		}

		if (Input.GetButtonUp("Fire3"))
		{
			isZooming = false;
		}

		if (isZooming)
			PerformZoom();
		else
			PerformUnzoom();
		/*
		foreach(var hold in holdsters)
		{
			switch(hold.gun.state)
			{
				case Gun.State.Fire:
				case Gun.State.Load:
				case Gun.State.Loading:
				case Gun.State.Loaded:
					PerformZoom();
					break;
				default:
					if (hold.gun.fire)
					{
						PerformZoom();
						break;
					}

					PerformUnzoom();
					break;
			}
		}*/

	}

	private void PerformZoom()
	{
		//cameraMount.transform.position = Vector3.Slerp(head.position, zoomPoint.position, Time.deltaTime * smoothing);
		var cam = Camera.main;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * smoothing);
	}

	private void PerformUnzoom()
	{
		//cameraMount.transform.position = Vector3.Slerp(zoomPoint.position, head.position, Time.deltaTime * smoothing);
		var cam = Camera.main;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal, Time.deltaTime * smoothing);
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

        //SceneManager.LoadScene("GameOver");
        
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


