using UnityEngine;
using System.Collections;

[AddComponentMenu("Small World/Gun")]
public class Gun : BehaviorBase
{
	public enum State { Ready, Load, Loading, Loaded, Fire, Reload, Reloading, Reloaded, Empty }

	public State state;
	public Ammo[] ammo;
	public float fireRate = 0.33f; // rate in t
	public float reloadTime = 5;
	public int clipSize = 60;
	public int currentClip = 60;
	public float changeTime = 0.5f;
	public float range = 10000;
	public Transform gunChamber;
	public Transform hand;
	private Bullet chamber = null;
	private float resumeTime;
	private Ammo currentAmmo;
	private bool fire;
	private Animator anim;


	// Use this for initialization
	void Start ()
	{
		Ensure(gunChamber);
		this.Assert(ammo.Length > 0, "No ammo assigned!");
		currentAmmo = ammo[0];
		Ensure(currentAmmo.bulletType, "Invalid Ammo Assigned!");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetButtonDown("Fire1"))
		{
			fire = true;
		}

		if (Input.GetButtonUp("Fire1"))
		{
			fire = false;
		}
		switch(state)
		{
			case State.Ready:
				if (fire)
				{
					state = State.Load;
				}

				if (Input.GetButtonDown("Fire2"))
				{
					ChangeAmmo();
				}
			break;

			case State.Load:
			resumeTime = Time.time + fireRate;

			if (--currentClip < 0)
			{
				state = State.Reload;
				break;
			}

			var bulletObj = Spawner.Spawn(currentAmmo.bulletType.prefab, false);
			chamber = bulletObj.GetComponent<Bullet>();
			anim.SetBool("Fire", true);
			state = State.Loading;
			break;

			case State.Loading:
			if (Time.time < resumeTime)
				return;

			state = State.Loaded;
			break;

			case State.Loaded:
			if (!chamber)
				state = State.Ready;
			else
				state = State.Fire;
			break; 

			case State.Fire:
				anim.SetBool("Fire", false);
				chamber.enabled = true;
				chamber = null; // Make ready for the next bullet
				//fire = false;
				state = State.Ready;
			break;

			case State.Reload:
			resumeTime = Time.time + reloadTime;
			// TODO - Kick off animation
			state = State.Reloading;			
			break;

			case State.Reloading:
			if (Time.time < resumeTime)
				break;

			state = State.Reloaded;
			break;

			case State.Reloaded:
			currentClip = clipSize;
			currentAmmo.clips--;

			if (currentAmmo.clips <= 0)
			{
				state = State.Empty;
				break;
			}
			// Todo - cleanup animation and sound 
			state = State.Ready;
			break;

			case State.Empty:
			if (currentAmmo.clips > 0)
			{
				state = State.Reload;
			}
			break;
		}
	}

	void FixedUpdate()
	{
		// http://answers.unity3d.com/questions/13022/aiming-gun-at-cursor.html
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

		Vector3 target;
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
			target = hit.point;
		else
			target = ray.GetPoint(100);

		hand.LookAt(hit.point);
		gunChamber.LookAt(hit.point);
	}

	void RecoilStart()
	{
		
	}

	void RecoilEnd()
	{
		if (fire)
			anim.SetBool("Fire", true);
	}


	public void ChangeAmmo()
	{
		// TODO!!
	}

	public void Reload() 
	{
		state = State.Reload;
	}
}

