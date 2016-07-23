//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

//This is just a basic timebomb class. 
//Set the timer, it'll count down to 0, then disable the object
public class AutoDisableTimer : MonoBehaviour 
{
	public float timer;
	
	// Use this for initialization
	void Start () 
	{
		//Destroy(gameObject,timer);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(timer > 0)
		{	
			timer -= Time.deltaTime;
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
