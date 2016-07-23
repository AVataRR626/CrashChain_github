//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour 
{

	public LayerMask dragSurfaceMask;
	public bool raycastMode = false;
	public float racastRadius = 1;

	private bool hitSwitch = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(raycastMode)
		{
			if(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
				TrackMouse();
		}


		if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
		{
			hitSwitch = false;
		}

	}

    void OnMouseEnter()
    {
        hitSwitch = true;
    }

    void OnMouseExit()
    {
        hitSwitch = false;
    }

	void OnMouseDrag()
	{
        Debug.Log("MouseDrag:Dragging");
		if(!raycastMode && hitSwitch)
			TrackMouse();
	}

	void TrackMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 1000, dragSurfaceMask))
		{
			if(raycastMode)
			{
				if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
					hitSwitch = (Vector3.Distance(transform.position,hit.point) <= racastRadius);
			}
			else
			{
				hitSwitch = true;
			}

			if(hitSwitch)
			{
				transform.position = hit.point;
			}
		}
	}
}
