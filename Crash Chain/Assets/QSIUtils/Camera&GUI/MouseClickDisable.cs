//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

//Disable object list when clicking..

using UnityEngine;
using System.Collections;

public class MouseClickDisable : MonoBehaviour 
{
	public GameObject [] disableList;
	protected bool disableMode = false;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.GetMouseButtonUp(0) && disableMode)
		{
			ActivateList(true);
			disableMode = false;
		}

		if(disableMode)
		{
			ActivateList(false);
		}

	}

	void OnMouseDown()
	{
		disableMode = true;
	}

	void ActivateList(bool mode)
	{
		if(disableList != null)
			for(int i = 0; i < disableList.Length; i++)
			{
				if(disableList[i] != null)
				{

					disableList[i].SetActive(mode);
				}
			}
	}
}
