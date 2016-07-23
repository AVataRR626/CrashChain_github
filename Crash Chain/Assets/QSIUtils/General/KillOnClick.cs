//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class KillOnClick : MonoBehaviour 
{
	public bool armed = true;//if set to false, this prevents instalkills on spawn
							//i.e. command beacons.
	public KeyCode killButton = KeyCode.Mouse0;
	public KeyCode secondaryKillButton = KeyCode.Mouse1;
	public bool raycastMode = false;
	public LayerMask cullingMask;

	protected bool killSwitch = false;

	// Update is called once per frame
	void Update () 
	{

		//Raycast mode is neccessary because the OnMouseOver callback
		//starts to become unreliable when there are too many overlapping colliders
		//in the scene. This becomes problematic whith RTS units and their radar spheres.
		if(raycastMode && (Input.GetKey(killButton) || Input.GetKey(secondaryKillButton)))
		{
			//Debug.Log ("KillOnClick: " + Input.mousePosition);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1000, cullingMask))
			{
				//Debug.Log ("KillOnClick: Hit Something!");
				Debug.DrawLine(ray.origin, hit.point);

				if(hit.collider == GetComponent<Collider>())
				{
					Debug.Log ("KillOnClick:killSwitch=true");
					killSwitch = true;
				}
			}
		}

		if(killSwitch)
			KillObject();
	}

	protected void ArmingCheck()
	{
		if(Input.GetKeyUp(killButton) || Input.GetKeyUp(secondaryKillButton))
		{
			armed = true;
		}
	}

	void OnMouseOver()
	{
		if(!raycastMode)
		{	Debug.Log ("KillOnClick:OnMouseOver");
			killSwitch = true;
		}
	}
	
	public void KillObject()
	{
		if(armed)
		{	Debug.Log ("KillOnClick:KillObject()");
			Destroy (gameObject);
		}
	}
}
