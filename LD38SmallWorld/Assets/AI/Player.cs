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
	public float minAimDistance = 20f;
	public bool isZooming;
	public bool dead {
		get { return health.dead; }
	}

	internal Health health;

	void Awake()
	{
		Ensure(head);
		Ensure(zoomPoint);
		Ensure(cameraMount);
		health = GetComponent<Health>();
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
	}

	private void PerformZoom()
	{
		//cameraMount.transform.position = Vector3.Lerp(head.position, zoomPoint.position, Time.deltaTime * smoothing);
		var cam = Camera.main;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * smoothing);
	}

	private void PerformUnzoom()
	{
		//cameraMount.transform.position = Vector3.Lerp(zoomPoint.position, head.position, Time.deltaTime * smoothing);
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


