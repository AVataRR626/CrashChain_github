/*
 * Quantum Shade Interactive
 * General Utilities
 * 
 * Authors:
 * 1) Matt Cabanag
 * 2) ..
 * 3) ..
 * 
 */

using UnityEngine;
using System.Collections;

public class TouchJoystick : MonoBehaviour 
{
	public float axisFactor = 0.1f;
	public Camera raycastCam;

	public float XAxis
	{
		get
		{
			return xAxis*axisFactor;
		}
	}

	public float YAxis
	{
		get
		{
			return yAxis*axisFactor;
		}
	}

	public TextMesh debugDisplay;
	private float xAxis;
	private float yAxis;
	private Vector2 touchDown;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		TrackAxis();
	}

	void TrackAxis()
	{
		if(Input.touchCount > 0)
		{
			Ray ray = raycastCam.ScreenPointToRay(Input.touches[0].position);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			

			if(hit.collider == GetComponent<Collider>())
			{
				if(Input.touches[0].phase == TouchPhase.Began)
				{
					touchDown = Input.touches[0].position;
				}
				else
				{	xAxis = Input.touches[0].position.x - touchDown.x;
					yAxis = Input.touches[0].position.y - touchDown.y;
				}
			}
			else
			{
				xAxis = 0;
				yAxis = 0;
			}

		}
		else
		{
			xAxis = 0;
			yAxis = 0;
		}
		
		//Debug.Log ("TouchJoystick:" + XAxis + "|" + YAxis);
		
		if(debugDisplay != null)
			debugDisplay.text = "TouchJoystick:" + XAxis + "|" + YAxis;
	}
}
