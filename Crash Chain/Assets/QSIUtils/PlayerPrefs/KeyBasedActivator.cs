using UnityEngine;
using System.Collections;

public class KeyBasedActivator : MonoBehaviour
{
    public string targetKey = "puzzle_4_1";

    public bool matchKeyValue = false;
    public int matchValue = 0;

    public GameObject[] activateList;
    public GameObject[] deactivateList;

	// Use this for initialization
	void Start ()
    {
	    if(PlayerPrefs.HasKey(targetKey))
        {
            if(!matchKeyValue)
            {
                Match();
            }
            else
            {
                if(PlayerPrefs.GetInt(targetKey) == matchValue)
                {
                    Match();
                }
                else
                {
                    NoMatch();
                }
            }
        }
        else
        {
            NoMatch();
        }
	}

    public void Match()
    {
        GenUtils.SetActiveObjects(ref activateList, true);
        GenUtils.SetActiveObjects(ref deactivateList, false);
    }

    public void NoMatch()
    {
        GenUtils.SetActiveObjects(ref activateList, false);
        GenUtils.SetActiveObjects(ref deactivateList, true);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    
}
