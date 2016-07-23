//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class ClickMessage : RaycastClickable 
{
	public string message;
	public int intParam;
	public GameObject [] targets;
	//public bool oneSendPerClick = true;

	private bool sendSwitch = false;
	private bool armed = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckRaycast();

		if(raycastMode)
		{
			if(hitSwitch)
			{
				if(clickSwitch)
					armed = true;
			}
			else
			{
				armed = false;
			}

			if(armed && hitSwitch)
			{
				if(!clickSwitch)
					sendSwitch = true;
			}

			if(sendSwitch)
			{	Shout ();
				sendSwitch = false;
				armed = false;
			}
		}
	}

	void OnMouseUp()
	{
		if(!raycastMode)
			Shout();
	}

	void Shout()
	{
		/*
		foreach(GameObject target in targets)
			if(target != null)
				target.SendMessage(message);*/

		GenUtils.Shout (ref targets, message, intParam);
	}

}
