using UnityEngine;
using System.Collections;

public class EscLvlLoader : MonoBehaviour 
{
	
	public string levelName;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//Debug.Log("MOUSE DOWNED ON LEVEL BUTTON!");
			Application.LoadLevel(levelName);
		}
	}
}
