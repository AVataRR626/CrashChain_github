using UnityEngine;
using System.Collections;

public class HeightRandomiser : MonoBehaviour 
{
	public float range = 3;

	// Use this for initialization
	void Start () 
	{
		Vector3 offset = new Vector3();
		offset.y = Random.Range(-range,range);
		transform.position += offset;
	}

}
