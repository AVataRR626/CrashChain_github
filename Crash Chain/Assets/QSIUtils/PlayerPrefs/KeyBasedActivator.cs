using UnityEngine;
using System.Collections;

public class KeyBasedActivator : MonoBehaviour
{
    public string targetKey = "puzzle_4_1";

    public GameObject[] activateList;
    public GameObject[] deactivateList;

	// Use this for initialization
	void Start ()
    {
	    if(PlayerPrefs.HasKey(targetKey))
        {
            GenUtils.SetActiveObjects(ref activateList, true);
            GenUtils.SetActiveObjects(ref deactivateList, false);
        }
        else
        {
            GenUtils.SetActiveObjects(ref activateList, false);
            GenUtils.SetActiveObjects(ref deactivateList, true);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    
}
