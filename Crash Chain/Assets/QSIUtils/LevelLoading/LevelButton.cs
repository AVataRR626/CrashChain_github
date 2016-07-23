using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
	public string levelName;

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
		//Debug.Log("MOUSE DOWNED ON LEVEL BUTTON!");
		LoadLevel(levelName);
	}
	
	void OnMouseEnter()
	{
		//Debug.Log("MOUSE ENTERED ON LEVEL BUTTOn!");
	}

	public void LoadLevel(string lname)
	{
		Application.LoadLevel(lname);
	}
}
