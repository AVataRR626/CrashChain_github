//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class KeypressActivator : MonoBehaviour 
{
	//these need to be the same size.
	public KeyCode cycleKey = KeyCode.Tab;
	public KeyCode[] triggers;
	public GameObject[] targets;
	public int activeIndex = 0;
	public bool pagingMode = true;

	private int lastActiveIndex = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(triggers.Length > 0 && triggers.Length == targets.Length)
		{
			if(Input.GetKeyDown(cycleKey))
			{   activeIndex++;
			}

			if(activeIndex >= triggers.Length)
			{
				activeIndex = 0;
			}

			for(int i = 0; i < triggers.Length; i++)
			{
				if(Input.GetKey(triggers[i]))
				{
					activeIndex = i;
				}
				else if(pagingMode && i != activeIndex)
				{
					targets[i].SetActive(false);
				}
			}

			if(lastActiveIndex != activeIndex)
				targets[activeIndex].SetActive(true);
		}

		lastActiveIndex = activeIndex;
	
	}
}
