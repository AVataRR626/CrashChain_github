using UnityEngine;
using System.Collections;

public class TimedLevelLoad : MonoBehaviour 
{
	public float timer = 30;
	public string level = "IntroMenu";

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(timer > 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			Application.LoadLevel(level);
		}
	
	}
}
