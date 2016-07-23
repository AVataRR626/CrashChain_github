using UnityEngine;
using System.Collections;

public class IntroOrthoCamZoom : MonoBehaviour 
{
	public float startZoom = 2;
	public float endZoom = -1;
	public float zoomRate = 1;
	public float timeLimit = 2.5f;

	private float currentZoom;
	private float clock = 0;

	// Use this for initialization
	void Start () 
	{
		currentZoom = startZoom;

		if(endZoom <= 0)
			endZoom = Camera.main.orthographicSize;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentZoom = Mathf.Lerp (currentZoom,endZoom,Mathf.Abs(clock));
		Camera.main.orthographicSize = currentZoom;

		timeLimit -= Time.deltaTime;
		clock += Time.deltaTime*zoomRate;

		if(timeLimit <= 0)
		{
			Camera.main.orthographicSize = endZoom;
			Destroy (this);
		}

	}
}
