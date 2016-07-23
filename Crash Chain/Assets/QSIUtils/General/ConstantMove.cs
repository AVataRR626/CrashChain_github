//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class ConstantMove : MonoBehaviour 
{
	public Vector3 direction = new Vector3(0,2,0);
	public float speed = 3;


	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed*Time.deltaTime);
	}
}
