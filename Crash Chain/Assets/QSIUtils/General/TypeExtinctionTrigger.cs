using UnityEngine;
using System.Collections;

public class TypeExtinctionTrigger : MonoBehaviour 
{
	//this component activates an object if
	//another certain type of object disappears
	//completely form the game board - i.e. extinct.
	
	public string unitTag;
	public float checkInterval = 2.0f;
	public GameObject triggerObject;
	
	private float checkClock = 2.0f;
	
	private GameObject [] trackedUnits;
	
	  
	
	// Use this for initialization
	void Start () 
	{
		
	}
			
	int countTrackedObjects()
	{
		trackedUnits = GameObject.FindGameObjectsWithTag(unitTag);
		
		if(trackedUnits != null)
		{
			return trackedUnits.Length;
		}
		return 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(checkClock <= 0)
		{
			if(countTrackedObjects() == 0 && triggerObject != null)
			{
				
				ActivateObjects(triggerObject.transform);
				
				/*
				triggerObject.active = true;
				
				foreach(Transform child in triggerObject.transform)
				{
					child.gameObject.active = true;
				}*/
			}
			checkClock = checkInterval;
		}
		else
		{
			checkClock -= Time.deltaTime;
		}
		
	}
	
	void ActivateObjects(Transform root)
	{
		root.gameObject.active = true;
		
		foreach(Transform child in root)
		{
			ActivateObjects(child);
		}
	}
}
