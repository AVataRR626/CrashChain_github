using UnityEngine;
using System.Collections;

public class MovesMonitor : MonoBehaviour
{
    public static MovesMonitor instance;

    public bool unlimitedMoves = false;
    public int moveLimit = 3;
    public int moveLimitInc = 3;
    public GameObject [] triggerObjects;

    OverchargeMonitor ocm;
    bool triggerSwitch = false;

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
	        if(moveLimit < 0)
            {
                GenUtils.SetActiveObjects(ref triggerObjects, true);
                triggerSwitch = true;
            }
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
