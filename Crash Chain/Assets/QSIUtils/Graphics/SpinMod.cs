//General QSI Utility
//
//Music Manager.cs
//-
//
//by 
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpinClick))]
public class SpinMod : MonoBehaviour 
{	
	public GameObject syncObject;
	public float modThreshold = 2;
	public int autoSpinInc = 10;
	public float autoSpinSpeed = 0.2f;
	public string spinSyncMethod = "SpinSyncMain";
	public string syncModTag = "";
	
	private SpinClick mySpinClick;
	private float accumulator;
	private int autoSpinCount = 0;
	private bool autoSpin = false;
	private float autoSpinDirection = 0;
	
	// Use this for initialization
	void Start () 
	{
		mySpinClick = GetComponent<SpinClick>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(syncObject == null)
			syncObject = GameObject.FindGameObjectWithTag(syncModTag);

		if((mySpinClick != null || autoSpin) && syncObject != null)
		{
			
			if(autoSpin)
			{
				accumulator += autoSpinSpeed;
				mySpinClick.transform.Rotate(0,autoSpinSpeed*30*-autoSpinDirection,0);
			}
			else
				accumulator += Mathf.Abs (mySpinClick.ScrollDelta)*Time.deltaTime;
			

			if(accumulator > modThreshold)
			{
				
				int delta = 1;
				
				if(mySpinClick.ScrollDelta < 0 || autoSpinDirection < 0)
				{
					delta = -1;
				}


				syncObject.SendMessage(spinSyncMethod,delta,SendMessageOptions.RequireReceiver);
				accumulator = 0;
				
				if(autoSpinCount < autoSpinInc - 1)
				{
					autoSpinCount++;
				}
				else
				{
					autoSpinCount = 0;
					autoSpin = false;
					autoSpinDirection = 0;
				}
			}
		}
	}
	
	public void AutoSpin(int dir)
	{
		autoSpinDirection = dir;
		autoSpin = true;
	}
}

