//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SmoothLookAt))]
public class LookAtTarget : MonoBehaviour 
{
	public Transform target;

	private SmoothLookAt mySmoothLooker;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
			mySmoothLooker.target = target;
			//transform.LookAt (target.position);
		}
	}
}
