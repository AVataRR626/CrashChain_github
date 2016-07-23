using UnityEngine;
using System.Collections;

public class SpinClick : MonoBehaviour 
{
	public float dragFactor = 0.25f;

	private Vector2 mouseDown;
	private Vector2 mouseLoc;
	private float scrollDelta;

	private Vector2 prevMouse;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mouseLoc = Input.mousePosition;

		if(Input.GetMouseButtonDown(0))
		{
			mouseDown = Input.mousePosition;
			mouseLoc = Input.mousePosition;
			prevMouse = mouseLoc;
			scrollDelta = 0;
			//Debug.Log ("SpinClick:OnMouseDrag:" + scrollDelta);
		}
		else if(!Input.GetMouseButton(0))
		{
			scrollDelta = 0;
		}
	}

	void OnMouseDown()
	{
		mouseDown = Input.mousePosition;
		mouseLoc = Input.mousePosition;
		prevMouse = mouseLoc;
		scrollDelta = 0;
	}

	void OnMouseDrag()
	{
		mouseLoc = Input.mousePosition;
		
		scrollDelta = mouseLoc.y - prevMouse.y;
		//scrollDelta = mouseLoc.y - mouseDown.y;

		//Debug.Log ("SpinClick:OnMouseDrag:" + scrollDelta);



		transform.Rotate(0,-scrollDelta*dragFactor*Time.deltaTime*100,0);
		prevMouse = mouseLoc;
	}

	public float ScrollDelta
	{
		get
		{
			return scrollDelta;
		}
	}
}
