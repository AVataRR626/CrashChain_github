using UnityEngine;
using System.Collections;

//By Matt Cabanag
//
//Activate object tree(s) when the player enters the trigger
//or some other event happens.

public class TriggerActivator : MonoBehaviour 
{
	public string playerTag = "Team0";
	public string activateMessage = "";
	public float idleTimer = 0;	
	public bool activateOnTimerEnd;	
	public bool deactivateMode = false;
	public Transform [] targets;
	
	public bool activateOnDeathwatchExtinction;
	public GameObject [] deathWatch;
	
	public bool deactivateOnTrigger = true;
	public bool killOnTrigger = false;
	public float selfDeactivateClock = 0;
	private bool selfDeactivateSwitch = false;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(idleTimer <= 0)
		{
			if(activateOnTimerEnd)
			{
				ActivateTargets();
			}
		}
		else
		{
			idleTimer -= Time.deltaTime;
		}
		
		//don't deactivate the trigger object immediately
		//for some reason, unity deactivation doesn't work if
		//the object doing the deactivating is inactive itself in the same frame.
		if(selfDeactivateSwitch)
		{
			if(selfDeactivateClock <= 0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				selfDeactivateClock -= Time.deltaTime;
				ActivateTargets();
			}
			
		}
		
		//activate targets whent all of the death watch list
		//has dieeedd!
		if(activateOnDeathwatchExtinction)
		{
			bool allDead = true;
			
			foreach(GameObject s in deathWatch)
			{
				if(s != null)
				{
					allDead = false;
				}
			}
			
			if(allDead)
			{
				//Debug.Log ("All Dead!");
				ActivateTargets();
			}
		}
	
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(idleTimer <= 0)
		{
			ActivateTargets(other);
			
		}
		
	}
	
	void ActivateTargets(Collider other)
	{
		if(other.tag == playerTag && targets.Length > 0)
		{
			foreach(Transform t in targets)			
			{
				if(t != null)
				{
					t.gameObject.SetActive(!deactivateMode);
					
					if(activateMessage != "")
					{
						t.gameObject.SendMessage(activateMessage,SendMessageOptions.DontRequireReceiver);
					}
					
					//ActivateChildren(t);
				}

			}
			
			if(deactivateOnTrigger)
			{
				selfDeactivateSwitch = true;
			}

			if(killOnTrigger)
			{
				Destroy (gameObject);
			}			
		}
	}
	
	void ActivateTargets()
	{
		if(targets.Length > 0)
		{
			//Debug.Log (name + ": activatingTargets!");
			
			foreach(Transform t in targets)			
			{
				
				if(t != null)
				{
					//Debug.Log (name + ": activating: " + t.name + " " + !deactivateMode);
					t.gameObject.SetActive(!deactivateMode);	
					
					if(activateMessage != "")
					{
						t.gameObject.SendMessage(activateMessage,SendMessageOptions.DontRequireReceiver);
					}
					
					ActivateChildren(t);
				}

			}
			
			if(deactivateOnTrigger)
			{
				//Debug.Log (name + ": NOW DEACTIVATING SELF");
				selfDeactivateSwitch = true;
			}
			
		}
	}	
	
	void ActivateChildren(Transform root)
	{
		
		//Debug.Log (name + ": activating: " + root.name + " " + !deactivateMode);
		root.gameObject.SetActive(true);
		
		foreach(Transform child in root)	
		{
			child.gameObject.SetActive(!deactivateMode);
			
			if(activateMessage != "")
			{
				child.gameObject.SendMessage(activateMessage,SendMessageOptions.DontRequireReceiver);
			}
			
			ActivateChildren(child);
		}
	}
	
	public static void ActivateChildren(Transform root, bool mode)
	{
		
		//Debug.Log (name + ": activating: " + root.name + " " + !deactivateMode);
		root.gameObject.SetActive(mode);
		
		foreach(Transform child in root)	
		{
			child.gameObject.SetActive(mode);
			
			
			ActivateChildren(child, mode);
		}
	}		
}
