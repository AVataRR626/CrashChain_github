using UnityEngine;
using System.Collections;

public class MovesMonitor : MonoBehaviour
{
    public static MovesMonitor instance;

    [Header("Trigger Settings")]
    public bool unlimitedMoves = false;
    public int moveLimit = 3;
    public int warningTrigger = 2;
    public int moveLimitInc = 3;
    public GameObject [] triggerObjects;

    [Header("Warning Settings")]
    public GameObject[] warningObjects;
    public string[] warningMessages;
    public GameObject [] warningDisableList;

    OverchargeMonitor ocm;
    bool triggerSwitch = false;
    bool warningSwitch = false;

    // Use this for initialization
    void Start ()
    {
        instance = this;

        ocm = OverchargeMonitor.instance;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!unlimitedMoves && !triggerSwitch)
        { 
	        if(ocm.GetMoves() > moveLimit)
            {
                GenUtils.SetActiveObjects(ref triggerObjects, true);
                triggerSwitch = true;
            }
        }

        ManageWarnings();
    }

    public void ManageWarnings()
    {
        if (GetMovesLeft() <= warningTrigger)
        {
            int i = 0;
            if (!warningSwitch)
            {
                foreach (GameObject o in warningObjects)
                {
                    o.SetActive(true);
                    o.SendMessage(warningMessages[i], SendMessageOptions.DontRequireReceiver);
                    i++;
                }

                foreach (GameObject o in warningDisableList)
                    o.SetActive(false);

                warningSwitch = true;
            }
        }
        else
        {
            foreach (GameObject o in warningObjects)
                o.SetActive(false);

            warningSwitch = false;
        }
    }

    public int GetMovesLeft()
    {   
        int result = moveLimit - ocm.GetMoves();

        if (result >= 0)
            return result;

        return 0;
    }

    public void IncreaseMoveLimit()
    {
        moveLimit += moveLimitInc;
    }
}
