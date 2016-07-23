using UnityEngine;
using System.Collections;

public class KeyLvlLoader : MonoBehaviour 
{
	public KeyCode key = KeyCode.N;
	public string levelName;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if(Input.GetKeyDown(key))
		{
			//Debug.Log("MOUSE DOWNED ON LEVEL BUTTON!");
			Application.LoadLevel(levelName);
		}
	}

	public void LoadLevel(string ln)
	{
		Application.LoadLevel(ln);
	}
}
