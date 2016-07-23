using UnityEngine;
using System.Collections;

public class MainLogo : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Color curCol = new Color(1.0f,1.0f,1.0f,0.5f);
		GetComponent<Renderer>().material.color =  curCol;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
