using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineNode : MonoBehaviour 
{
	public Transform next;

	private LineRenderer lr;

	// Use this for initialization
	void Start () 
	{
		lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		lr.SetPosition(0,transform.position);

		if(next != null)
			lr.SetPosition(1,next.position);
	}
}
