/*
General QSI Utils, 

Cycles through screen orientations...

-Matt Cabanag
*/

using UnityEngine;
using System.Collections;

public class OrientationToggle : MonoBehaviour
{
    public ScreenOrientation [] stateList;
    public int currentState;

	// Use this for initialization
	void Start ()
    {
        currentState = 0;
        Screen.orientation = stateList[currentState];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Toggle()
    {

        currentState++;

        if (currentState >= stateList.Length)
            currentState = 0;

        Screen.orientation = stateList[currentState];
        
    }
}
