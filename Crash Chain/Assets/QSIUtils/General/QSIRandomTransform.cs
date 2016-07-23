using UnityEngine;
using System.Collections;

public class QSIRandomTransform : MonoBehaviour 
{
	public Vector3 positionRange = new Vector3(0,0,0);
	public Vector3 maxScale = new Vector3(2,2,2);
	public Vector3 minScale = new Vector3(0.5f,0.5f,0.5f);
	public Vector3 maxAngle = new Vector3(10,180,10);
	public Vector3 minAngle = new Vector3(0,180,0);

	// Use this for initialization
	void Start () 
	{
		Vector3 newPos = new Vector3();
		Vector3 newScale = new Vector3();
		Vector3 newRot = new Vector3();

		newPos.x = Random.Range (-positionRange.x,positionRange.x);
		newPos.y = Random.Range (-positionRange.y,positionRange.y);
		newPos.z = Random.Range (-positionRange.z,positionRange.z);
		transform.position = transform.position + newPos;

		newScale.x = Random.Range(minScale.x,maxScale.x);
		newScale.y = Random.Range(minScale.y,maxScale.y);
		newScale.z = Random.Range(minScale.z,maxScale.z);
		transform.localScale = newScale;

		newRot.x = Random.Range (minAngle.x,maxAngle.x);
		newRot.y = Random.Range (minAngle.y,maxAngle.y);
		newRot.z = Random.Range (minAngle.z,maxAngle.z);
		transform.Rotate (newRot);
	}

}
