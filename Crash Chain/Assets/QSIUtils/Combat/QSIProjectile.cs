using UnityEngine;
using System.Collections;

public class QSIProjectile : MonoBehaviour 
{
	public Transform target;
	public float range = 3;
	public float linearSpeed = 1;
	public float arrivalBuffer;
	public float Dist2Target
	{
		get
		{
			return dist2Target;
		}
	}
	
	private float dist2Target = 1000000;
	private float distanceTravelled;

	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
			dist2Target = Vector3.Distance(transform.position,target.position);

			if(dist2Target >= arrivalBuffer)
				transform.position = Vector3.MoveTowards(transform.position,target.position,linearSpeed*Time.deltaTime);
		}
		else
		{
			transform.Translate(linearSpeed * transform.forward);
		}

		distanceTravelled += Time.deltaTime * linearSpeed;

		if(distanceTravelled > range)
			Destroy(gameObject);
	
	}
}
