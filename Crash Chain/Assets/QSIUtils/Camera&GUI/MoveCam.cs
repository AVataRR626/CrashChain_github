//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class MoveCam : MonoBehaviour 
{

	public float wasdMovespeed = 10;
	public float clickMoveSpeedFactor = 1.5f;
	public Transform anchor;
	public string anchorTag = "";
	public float maxAnchorDistance = 30;
	public float clickmoveThreshold = 4;
	public Camera hudCam;
	public LayerMask clickable;
	public LayerMask exclusionMask;
	public KeyCode moveClickKey = KeyCode.Mouse0;
	public TouchJoystick myTouchStick;
	public bool invisibleOnWASD = false;

	protected Vector3 clickDestDown;
	protected Vector3 clickDestUp;
	protected int frameDelayCount = 5;
	protected float clickMoveSpeed;

	protected Vector2 v2MouseDown;
	protected Vector2 v2MouseUp;

	protected bool wasdMode = false;

	// Use this for initialization
	void Start () 
	{

		if(hudCam == null)
		{
			GameObject c = GameObject.FindGameObjectWithTag("HUDCam");

			if(c != null)
			{
				hudCam = c.GetComponent<Camera>();
			}
		}

		if(anchorTag != "")
		{	
			GameObject a = GameObject.FindGameObjectWithTag(anchorTag);
			if(a != null)
				anchor = a.transform;
		}

		if(anchor != null)
		{	transform.position = anchor.position;
			wasdMode = true;
		}

		clickDestUp = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		WASDMode();
		TrackCursor();
	}

	void WASDMode()
	{
		float xspeed = Input.GetAxis ("Horizontal") * Time.deltaTime * wasdMovespeed;
		float yspeed = Input.GetAxis ("Vertical") * Time.deltaTime * wasdMovespeed;

		if(Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)
			wasdMode = true;

		//touchstick gets priority
		if(myTouchStick != null)
		{
			if(myTouchStick.XAxis != 0 || myTouchStick.YAxis != 0)
			{	
				xspeed = myTouchStick.XAxis * Time.deltaTime * wasdMovespeed;
				yspeed = myTouchStick.YAxis  * Time.deltaTime * wasdMovespeed;
				wasdMode = true;
			}
		}

		if(wasdMode && anchor != null)
		{
			if(xspeed == 0 && yspeed == 0)
			{
				transform.position = anchor.position;
			}
			else if(Vector3.Distance(transform.position,anchor.position) > maxAnchorDistance)
			{
				xspeed *= -1;
				yspeed *= -1;
			}

		}

		if(invisibleOnWASD)
			GetComponent<Renderer>().enabled = !wasdMode;

		
		/*
		if(xspeed != 0)
			transform.Translate(new Vector3(xspeed*moveSpeed,0,0));

		if(yspeed != 0)
			transform.Translate(new Vector3(0,0,yspeed*moveSpeed));
		*/

		if( Mathf.Abs (xspeed) > 0 || Mathf.Abs (yspeed) > 0)
		{
			Vector3 delta = new Vector3(xspeed,0,yspeed);
			
			Vector3 camPos = Camera.main.transform.position;
			Quaternion camRot = Camera.main.transform.rotation;
			
			delta = transform.rotation * delta;
			transform.rotation = new Quaternion(0,camRot.y,0,camRot.w);

			if(anchor != null)
			{	//transform.position = anchor.transform.position + (delta* 20);

				Vector3 newPos = anchor.transform.position + (delta* 20);

				transform.position = Vector3.MoveTowards(transform.position, newPos, 10);
				//Debug.Log ("MoveCam:" + delta);
			}
			else
				transform.position += delta;

			clickDestUp = transform.position;
		}
	}

	void TrackCursor()
	{
		//Debug.Log ("RTSCursor: " + Input.mousePosition);
		if((v2MouseDown - v2MouseUp).magnitude <= clickmoveThreshold)
		{
			clickMoveSpeed = Vector3.Distance(transform.position,clickDestUp)*clickMoveSpeedFactor;
			MoveToClickUp(clickMoveSpeed);
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		
		//stop operations if you hit any of the excluded masks.
		if (Physics.Raycast(ray, out hit, 1000, exclusionMask))
		{	
			frameDelayCount = 15;
			return;
		}

		if(hudCam != null)
		{
			Ray hudRay = hudCam.ScreenPointToRay(Input.mousePosition);

			//stop operations if you hit any of the excluded masks.
			if (Physics.Raycast(hudRay, out hit, 1000, exclusionMask))
			{	
				frameDelayCount = 15;
				return;
			}

		}

		if(Input.GetKeyDown (moveClickKey))
		{
			if (Physics.Raycast(ray, out hit, 1000, clickable))
			{	
				//Debug.DrawLine(ray.origin, hit.point);
				clickDestDown = hit.point;// + inputParameters.surfaceOffset;
			}

			v2MouseDown = Input.mousePosition;
			wasdMode = false;
		}

		if(Input.GetKeyUp(moveClickKey) && frameDelayCount <= 0)
		{
			if (Physics.Raycast(ray, out hit, 1000, clickable))
			{	
				//Debug.DrawLine(ray.origin, hit.point);
				clickDestUp = hit.point;// + inputParameters.surfaceOffset;
				v2MouseUp = Input.mousePosition;
			}
			wasdMode = false;
		}

		if(frameDelayCount > 0)
			frameDelayCount--;
	}

	public void MoveToClickUp(float speed)
	{
		transform.position = Vector3.MoveTowards(transform.position, clickDestUp, speed*Time.deltaTime);
	}

}
