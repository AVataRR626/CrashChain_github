//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class MouseOverDisable : MonoBehaviour 
{
	public GameObject [] disableList;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseOver()
	{
		Debug.Log ("MouseOverDisable:Hovering"+name);
		ActivateList(false);
	}

	void OnMouseExit()
	{
		Debug.Log ("MouseOverDisable:Exit"+name);
		ActivateList(true);
	}

	void ActivateList(bool mode)
	{
		if(disableList != null)
			for(int i = 0; i < disableList.Length; i++)
			{
				if(disableList[i] != null)
				{	
					disableList[i].SetActive(mode);
					//Debug.Log ("Deactivating:"+disableList[i].name);
				}
			}
	}
}
