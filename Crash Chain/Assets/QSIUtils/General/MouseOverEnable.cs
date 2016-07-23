//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class MouseOverEnable : MonoBehaviour 
{
	public GameObject [] targetList;
	

	// Use this for initialization
	void Start () 
	{
		ActivateList(false);

	}

	void OnDisable()
	{
		ActivateList(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseOver()
	{
		Debug.Log ("MouseOverDisable:Hovering"+name);
		ActivateList(true);
	}

	void OnMouseExit()
	{
		Debug.Log ("MouseOverDisable:Exit"+name);
		ActivateList(false);
	}

	void ActivateList(bool mode)
	{
		if(targetList != null)
			for(int i = 0; i < targetList.Length; i++)
			{
				if(targetList[i] != null)
				{	
					targetList[i].SetActive(mode);
					//Debug.Log ("Deactivating:"+disableList[i].name);
				}
			}
	}
}
