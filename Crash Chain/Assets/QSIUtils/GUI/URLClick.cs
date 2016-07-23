using UnityEngine;
using System.Collections;

public class URLClick : MonoBehaviour 
{

	
	public string url;

	// Use this for initialization
	void Start () 
	{
	
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseDown()
	{
		Application.OpenURL(url);
	}
}
