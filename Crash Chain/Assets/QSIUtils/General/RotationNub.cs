//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class RotationNub : MonoBehaviour 
{
	public Transform anchor;

	protected Vector3 anchorDelta;
	protected Vector3 prevAnchorPos;


	void Start()
	{

		if(anchor != null)
		{
			prevAnchorPos = anchor.position;
		}

	}

	// Update is called once per frame
	void Update ()
	{

		if(anchor != null)
		{
			anchorDelta = anchor.position - prevAnchorPos;
			transform.position += anchorDelta;

			//offset = anchor.position - transform.position;
			anchor.LookAt(transform.position);
			transform.LookAt(anchor.position);

			prevAnchorPos = anchor.position;
		}
		else
		{
			//no point in existing if you have no anchor.
			Destroy (gameObject);
		}


	}
}
