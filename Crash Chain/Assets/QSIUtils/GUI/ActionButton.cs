using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour 
{
	public GameObject triggetObj;
	public string actionMessage;
	
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
		if(triggetObj != null && actionMessage != null)
		{
			triggetObj.SendMessage(actionMessage);
		}
	}
}
