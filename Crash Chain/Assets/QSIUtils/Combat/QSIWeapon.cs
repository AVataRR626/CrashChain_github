using UnityEngine;
using System.Collections;

public class QSIWeapon : MonoBehaviour 
{
	public QSIKillable owner;
	public QSIKillable target;
	public float damage = 3;
	public float launchDelay = 0.5f;
	public float launchSpeed = 0.5f;
	public QSIDamager damageObject;
	public int ammo = 5;//negatives mean unlimited ammo.
	public bool lookAtTarget = true;

	private float launchClock = 0;

	// Use this for initialization
	void Start () 
	{
		if(owner == null)
			owner = transform.root.GetComponent<QSIKillable>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(lookAtTarget && target != null)
			transform.LookAt(target.transform.position);

		ClockTick();
	}

	void ClockTick()
	{
		if(launchClock > 0)
			launchClock -= Time.deltaTime;
		else
			launchClock = 0;
	}

	[ContextMenu("Fire")]
	public void Fire()
	{
		if(damageObject != null)
		{
			if(launchClock <= 0 && (ammo > 0 || ammo < 0))
			{
				QSIDamager b = Instantiate(damageObject,transform.position,transform.rotation) as QSIDamager;

				b.target = target;
				b.damage = damage;


				b.GetComponent<QSIProjectile>().linearSpeed = launchSpeed;

				launchClock = launchDelay;

				if(ammo > 0)
					ammo--;
			}
		}
	}
}
