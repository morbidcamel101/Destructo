using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

[AddComponentMenu("Small World/Player")]
[RequireComponent(typeof(Health))]
public class Player: CharacterBase
{
	public int score = 0;
	public Transform head;
	public Transform zoomPoint;
	public Transform cameraMount;
	public float smoothing = 5f;
	public float zoom = 20f;
	public float normal = 60f;
	public float minAimDistance = 20f;
	public bool isZooming;
	internal HitInfo aim = new HitInfo();
	//private ITarget zoomTarget;
	//private ITarget headTarget;

	void Awake()
	{
		Ensure(head);
		Ensure(zoomPoint);
		Ensure(cameraMount);
		//zoomTarget = new DynamicTarget(this.head, this.zoomPoint);
		//headTarget = new DynamicTarget(this.zoomPoint, this.head);
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
			StopFire("rocket");
		}


	}

	void FixedUpdate()
	{
		// http://answers.unity3d.com/questions/13022/aiming-gun-at-cursor.html
		var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)); // Camera.main.ScreenPointToRay(Input.mousePosition); 

		var distSqr = minAimDistance * minAimDistance;


		if (Physics.Raycast(ray, out aim.hit) && (aim.hit.point - transform.position).sqrMagnitude > distSqr)
		{
			if (aim.hit.transform.GetComponent<Thug>() != null)
			{
				var offset = aim.hit.point - aim.hit.transform.position;
				SetTarget(new DynamicTarget(transform, aim.hit.transform, offset));
			}
			else
			{
				SetTarget(new StaticTarget(aim.hit.point, (aim.hit.point - transform.position).normalized));
			}

		}
		else
		{
			var target = ray.GetPoint(1000);
			SetTarget(new StaticTarget(target, (target - this.transform.position).normalized));
		}
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
	}

	private void PerformZoom()
	{
		// TODO - Write head script rather
		//cameraMount.transform.localPosition = Vector3.Lerp(cameraMount.transform.localPosition, cameraMount.transform.localPosition + (zoomTarget.GetDirection(transform.position) * smoothing), Time.deltaTime);
		var cam = Camera.main;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * smoothing);
	}

	private void PerformUnzoom()
	{
		//cameraMount.transform.localPosition = Vector3.Lerp(cameraMount.transform.localPosition, cameraMount.transform.localPosition + (headTarget.GetDirection(transform.localPosition) * smoothing), Time.deltaTime);
		var cam = Camera.main;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal, Time.deltaTime * smoothing);
	}

	public override void SetTarget (ITarget target)
	{
		if (target.IsSame(aim.target))
		{
			aim.target.CopyFrom(target);
			target = aim.target;
		}
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

        // Set static score before opening next scene
        GameStatsHolder.scoreTotal = score;

        SceneManager.LoadScene("GameOver");
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


