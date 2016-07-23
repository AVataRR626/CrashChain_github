//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class ClickScale : RaycastClickable 
{
	public float scaleFactor = 0.85f;
	protected Vector3 origScale;


	// Use this for initialization
	void Start () 
	{
		origScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckRaycast();

		if(raycastMode)
		{
			if(clickSwitch)
			{
				ScaleShrink();
			}
			else
			{
				ScaleDefault();
			}
		}

	}

	void OnMouseDown()
	{
		ScaleShrink();
	}

	void OnMouseUp()
	{
		ScaleDefault();
	}

	void ScaleShrink()
	{
		transform.localScale = origScale*scaleFactor;
	}

	void ScaleDefault()
	{
		transform.localScale = origScale;
	}
}
