//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class MessageTicker : MonoBehaviour 
{
	public string message;
	public GameObject [] targets;
	public float messageInterval;
	public float messageClock;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(messageClock <= 0)
		{
			Shout ();
			messageClock = messageInterval;
		}
		else
		{
			messageClock -= Time.deltaTime;
		}
	}

	void Shout()
	{
		GenUtils.Shout(ref targets, message);
	}
}
