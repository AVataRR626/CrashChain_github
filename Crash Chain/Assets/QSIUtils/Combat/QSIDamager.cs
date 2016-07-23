using UnityEngine;
using System.Collections;

[RequireComponent(typeof(QSIProjectile))]
public class QSIDamager : MonoBehaviour 
{
	public QSIKillable target;
	public bool armed = true;
	public bool oneHitMode = true;
	public bool damageAreaMode = false;
	public float damage = 2;
	public float damageDistance = 0.5f;

	private QSIProjectile myProjectile;

	// Use this for initialization
	void Start () 
	{
		myProjectile = GetComponent<QSIProjectile>();
		if(target != null)
			myProjectile.target = target.transform;

		if(damageAreaMode)
			GetComponent<Collider>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(oneHitMode && armed)
		{	
			if(target != null && myProjectile.Dist2Target <= damageDistance)
			{
				target.Damage(damage);
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("QSIDamager: entered:  " + col.name);

		if(armed && col.transform != transform.root)
		{
			QSIKillable k = col.gameObject.GetComponent<QSIKillable>();
			if(k != null)
				k.Damage(damage);
		}
	}

	public void ArmDamage(bool arm)
	{
		armed = arm;
	}


}
