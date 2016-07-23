using UnityEngine;
using System.Collections;

public class RandomStartRotation : MonoBehaviour 
{
	public bool xAxis = false;
	public bool yAxis = false;
	public bool zAxis = false;

	// Use this for initialization
	void Start ()
	{
		if(xAxis)
			transform.Rotate (Random.Range(0,359),0,0);

		if(yAxis)
			transform.Rotate (0,Random.Range(0,359),0);

		if(zAxis)
			transform.Rotate (0,0,Random.Range(0,359));
	}

}
