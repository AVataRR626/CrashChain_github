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
    private float clock;
    public string disableMessage = "Reset";
	
	// Use this for initialization
	void Start () 
	{
        //Destroy(gameObject,timer);

        Reset();
    }
	
	// Update is called once per frame
	void Update () 
	{
		
		if(clock > 0)
		{
            clock -= Time.deltaTime;
		}
		else
		{
            gameObject.SendMessage(disableMessage, SendMessageOptions.DontRequireReceiver);            
			gameObject.SetActive(false);
		}
	}

    public void Reset()
    {
        clock = timer;
    }
}
