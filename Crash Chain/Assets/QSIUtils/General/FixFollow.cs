using UnityEngine;
using System.Collections;

/*
BounceBlaster Project
FixFollow.cs

Follows a fixed transform one for one, plus an offset.

Authors:
1) Matt Cabanag
2) ...
3) ...
  
*/

public class FixFollow : MonoBehaviour 
{
	public Transform subject;
	public Vector3 offset;
	public Transform rotationRef;
	public bool dieWithSubject = false;
	

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(subject != null)
		{
			transform.position = subject.position + offset;
		}
		else
		{
			if(dieWithSubject)
				Destroy(gameObject);//no need to exist if there's no subject
		}

		if(rotationRef != null)
		{
			Vector3 newRot = transform.rotation.eulerAngles;
			newRot.y = rotationRef.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler (newRot);
		}
	}
}
