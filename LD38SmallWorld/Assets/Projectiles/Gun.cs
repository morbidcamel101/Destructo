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
	public float reactionSpeed = 1f;
	public float strengthMultiplier = 1f;
	public BulletTargeting targeting;
    public AudioClip shootSound;

	private Bullet currentBullet = null;
	private float resumeTime;
	private Ammo currentAmmo;
	internal bool fire;
	private Animator anim;
	private ITarget target;
    private AudioSource audioSource;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    private CharacterBase character;

    // Use this for initialization
    void Start ()
	{
		Ensure(gunChamber);
		this.Assert(ammo.Length > 0, "No ammo assigned!");
		currentAmmo = ammo[0];
		Ensure(currentAmmo.bulletType, "Invalid Ammo Assigned!");
		anim = GetComponentInChildren<Animator>();
		character = GetComponentInParent<CharacterBase>();
		Ensure(character);
	}

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Reset(float strengthMultiplier)
    {
    	// LD38 BUG FIXED - I divided!?!?
		this.strengthMultiplier = strengthMultiplier;
		reactionSpeed = reactionSpeed * strengthMultiplier;
    	fireRate = Mathf.Clamp(fireRate / strengthMultiplier, 0.05f, 2f);
    }

    // Update is called once per frame
    void Update ()
	{
		switch(state)
		{
			case State.Ready:
				UpdateAnimatorAndSound(false,false);
				if (fire)
				{
					state = State.Load;
				}
			break;

			case State.Load:
				resumeTime = Time.time + fireRate;

				if (--currentClip < 0)
				{
					state = State.Reload;
					break;
				}

				var bulletObj = Spawner.Spawn(currentAmmo.bulletType.prefab, false, gunChamber.position, gunChamber.rotation);

				currentBullet = bulletObj.GetComponent<Bullet>();
				currentBullet.strengthMultiplier = this.strengthMultiplier;
				UpdateAnimatorAndSound(true, false);
				state = State.Loading;
			break;

			case State.Loading:
				if (Time.time < resumeTime)
					return;

				state = State.Loaded;
			break;

			case State.Loaded:
				if (!currentBullet)
					state = State.Ready;
				else
					state = State.Fire;
			break; 

			case State.Fire:
				
				currentBullet.Shoot(character, GetBulletTarget(target));

				SendMessageUpwards("OnFire", currentBullet, SendMessageOptions.RequireReceiver);
				currentBullet = null; // Make ready for the next bullet
				UpdateAnimatorAndSound(true, false);
				state = fire ? State.Load : State.Ready;
                break;

			case State.Reload:
				resumeTime = Time.time + reloadTime;
				UpdateAnimatorAndSound(false, true);
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
				UpdateAnimatorAndSound(false,false);
				if (currentAmmo.clips <= 0)
				{
					state = State.Empty;
					break;
				}
				// Todo - cleanup animation and sound 
				state = State.Ready;
			break;

			case State.Empty:
				UpdateAnimatorAndSound(false, false);
				if (currentAmmo.clips > 0)
				{
					state = State.Reload;
				}
			break;
		}

		if (target == null || !target.IsReady)
			return;

		var rotation =  Quaternion.LookRotation(target.Position - transform.position);
 		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * reactionSpeed);

 		//this.transform.LookAt(target);
		gunChamber.LookAt(target.Position);
	}

	public void UpdateAnimatorAndSound(bool recoil, bool reload)
	{
		recoil = recoil && !reload;
		if (anim) 
		{
			anim.SetBool(name+"_Recoil", recoil);
			anim.SetBool("Reload", reload); 
			anim.SetFloat("ReloadSpeed", (2f/reloadTime));
		}
		if (audioSource)
		{
			if (recoil)
			{
				float vol = Random.Range(volLowRange, volHighRange); // Good!
				audioSource.volume = vol;
				audioSource.PlayOneShot(shootSound, vol);
			}
			else
			{
				audioSource.volume = 0;
			}

			// TODO - Reload

			// TODO - Empty

		}
	}

	private ITarget GetBulletTarget(ITarget target)
	{
		switch(targeting)
		{
			case BulletTargeting.HeatSeeker:
				return target;

			default:
				return new StaticTarget(target.Position, target.GetDirection(gunChamber.position)); 
		}
	}


	public void ChangeAmmo()
	{
		// TODO!!
	}

	public void Reload() 
	{
		state = State.Reload;
	}

	public bool FireAt(ITarget target)
	{
		this.target = target;

		return Fire();
	}

	public bool Fire()
	{
		if (!target.IsReady)
			return false;

		fire = true;

        return state != State.Empty;
	}

	public void StopFire()
	{
		fire = false;
	}

	public void SetTarget(ITarget target)
	{
		// ToDO - Miss - random inaccuracy
		this.target = target;
	}
}

