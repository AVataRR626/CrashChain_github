using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FixFollow))]
public class Minimap : MonoBehaviour 
{
	public float xPos = 0;
	public float yPos = 0;

	//minimap's width and height as a percentage of screen dimensions.
	public float width = 100;
	public float height = 100;
	public string followTag;

	private FixFollow fxFlw;

	// Use this for initialization
	void Start () 
	{
		SyncScreen();

		GameObject tflw = GameObject.FindWithTag(followTag) as GameObject;
		fxFlw = GetComponent<FixFollow>();

		if(tflw != null && followTag != "")
		{	fxFlw.subject = tflw.transform;
		}
	
	}

	void SyncScreen()
	{
		gameObject.GetComponent<Camera>().pixelRect = new Rect(xPos, yPos, (width/100)*Screen.height, (height/100)*Screen.height);
	}

	void OnDrawGizmosSelected()
	{
		SyncScreen();
	}

}
