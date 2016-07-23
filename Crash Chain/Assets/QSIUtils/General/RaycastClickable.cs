using UnityEngine;
using System.Collections;

public class RaycastClickable : MonoBehaviour 
{
	public bool raycastMode = false;


	protected bool hitSwitch = false;
	protected bool clickSwitch = false;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckRaycast();
	}

	protected void CheckRaycast()
	{
		if(raycastMode)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;			


			if(Physics.Raycast(ray, out hit, 1000))
				hitSwitch = (hit.collider.gameObject == gameObject);
			else
				hitSwitch = false;

			clickSwitch = hitSwitch &&( Input.GetMouseButton(0) || Input.GetMouseButton(1));

		}
	}
}
