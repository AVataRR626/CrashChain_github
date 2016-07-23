

using UnityEngine;
using System.Collections;

//By Matt Cabanag
//
//Handle orbiting camera behaviour around a specific target


public class QSICameraOrbit : MonoBehaviour 
{	
	public Transform target;
	public string autofindTargetTag;
	public float distance;
	public float scrollWheelMultiplier = 600;
	public KeyCode primaryCamKey = KeyCode.Mouse2;
	public KeyCode secondaryCamKey = KeyCode.C;
	public KeyCode screenshotKey = KeyCode.Z;
	public KeyCode rotateLeftKey = KeyCode.Q;
	public KeyCode rotateRightKey = KeyCode.E;
	public float rotateKeySpeed = 48;
	
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float distanceMin = 20;
	public float distanceMax = 120;
	public float yMinLimit = 20;
	public float yMaxLimit = 80;
	
	public float idleTimer = 0;
	
	public float x = 0.0f;
	public float y = 0.0f;
	
	void OnDrawGizmosSelected()
	{
		//y = 45.0f;
		LateUpdate();	
	}
	
	// Use this for initialization
	void Start () 
	{
		if(autofindTargetTag != "")
		{
			target = GameObject.FindGameObjectWithTag(autofindTargetTag).GetComponent<Transform>();
		}

		if(GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().freezeRotation = true; 
		}
		
		//Vector3 angles = transform.eulerAngles;
		x = 0.0f;
		//y = 45.0f;
	}
	
	void Update()
	{
		if(idleTimer > 0)
		{
			idleTimer -= Time.deltaTime;
		}
		
		if(Input.GetKeyDown(screenshotKey))
		{
			string ssfname = "protocol_e_ss_"+System.DateTime.Now.ToString("yyMMddhhmmss")+".jpg";
			Application.CaptureScreenshot(ssfname);
			Debug.Log(ssfname);
		}
	}
	
	void LateUpdate () 
	{
		if(Input.GetKey (rotateLeftKey))
		{
			x += rotateKeySpeed * Time.deltaTime;
		}
		
		if(Input.GetKey (rotateRightKey))
		{
			x -= rotateKeySpeed * Time.deltaTime;
		}
		
		
		if(target != null && idleTimer <= 0)
		{
			
			if(Input.GetKey (primaryCamKey) || Input.GetKey (secondaryCamKey))
			{
				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
	        	y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			}
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			distance = Mathf.Clamp(distance,distanceMin,distanceMax);
				
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
        	Vector3 position = (rotation * new Vector3(0.0f, 0.0f, -distance)) + target.position;
			
			transform.rotation = rotation;
       		transform.position = position;
			
			float zoomMultiplier,camZoom;
			
			zoomMultiplier = Vector3.Distance(transform.position,target.transform.position); 
			
			if(zoomMultiplier > 10)
			{
				zoomMultiplier = 50;
			}
			
			if(zoomMultiplier < 1)
			{
				zoomMultiplier = 1;
			}
			
			distance -= Input.GetAxis("Mouse ScrollWheel")*zoomMultiplier;
		}
		

		
	}
	
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}
