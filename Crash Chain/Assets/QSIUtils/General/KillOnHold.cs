using UnityEngine;
using System.Collections;

public class KillOnHold : MonoBehaviour 
{
	public float timeThreshold = 3;
	public KeyCode triggerKey = KeyCode.Mouse0;

	float dragTimer = 0;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey (triggerKey))
			dragTimer += Time.deltaTime;

		if(dragTimer >= timeThreshold)
			Destroy (gameObject);

	}
}
