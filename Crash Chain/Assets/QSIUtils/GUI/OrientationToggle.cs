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
    public static int currentState;
    public static string orientationKey = "Orientation";

	// Use this for initialization
	void Start ()
    {
        currentState = PlayerPrefs.GetInt(orientationKey,0);
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

        PlayerPrefs.SetInt(orientationKey, currentState);
        
    }
}
