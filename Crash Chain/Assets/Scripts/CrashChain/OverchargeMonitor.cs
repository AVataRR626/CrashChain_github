using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OverchargeMonitor : MonoBehaviour
{
    public static OverchargeMonitor instance;

    public float timeLimit = 1.65f;    
    public int timerTrigger = 1;
    public CrashChainDynLevelSaver levelSaver;
    public PopulationCheck popChecker;
    public GameObject [] activateObjects;
    public GameObject [] disableObjects;
    public GameObject [] messageList;
    public string message;
    public int overchargeLimInc = 0;

    private int moveCount = 0;
    private float clock;
    private bool saveSwitch = false;
    

	// Use this for initialization
	void Start ()
    {
        instance = this;

        if(popChecker == null)
            popChecker = FindObjectOfType<PopulationCheck>();

        if (levelSaver == null)
            levelSaver = FindObjectOfType<CrashChainDynLevelSaver>();

        clock = timeLimit;
    }

	
	// Update is called once per frame
	void Update ()
    {
	    if(CrashLink.overchargeCount == timerTrigger)
        {       

            if (clock > 0)
            {

                //someone's trying to cheat! end the game now.
                if (Input.GetMouseButtonDown(0) && clock < timeLimit - 0.2f)
                    clock = 0;

                if (!saveSwitch)
                {
                    levelSaver.SaveLevel();
                    saveSwitch = true;
                }

                clock -= Time.deltaTime;
            }
            else
            {
                Trigger();
            }
        }
        else if (CrashLink.overchargeCount > timerTrigger)
        {
            Trigger();
        }
	}

    public int GetMoves()
    {
        return moveCount;
    }
    
    public void AddMove()
    {
        if(CrashLink.overchargeCount != timerTrigger)
            moveCount++;
    }

    public void AddMoves(int n)
    {
        moveCount += n;
    }

    public void Trigger()
    {
        //don't be defeated if the board is already cleared.. 
        if (popChecker != null)
            if (popChecker.GetPop() == 0)
            {
                //reset retry count, player has won!
                levelSaver.RegisterWin();
                return;
            }

        foreach (GameObject o in activateObjects)
            o.SetActive(true);

        foreach (GameObject o in disableObjects)
            o.SetActive(false);

        foreach (GameObject o in messageList)
            o.SendMessage(message);

        //increment retry count, player has lost...
        levelSaver.ReryIncrement();

        //ResetOvercharge();
    }

    public void ResetOvercharge()
    {
        Debug.Log("--------------RESETTING OVERCHARGE");


        timerTrigger += overchargeLimInc;

        if(overchargeLimInc == 0)
            CrashLink.overchargeCount = 0;
    }

    public void AddToClock(float t)
    {
        if (clock + t < timeLimit)
            clock += t;
        else
            clock = timeLimit;
    }

    public int RemainingOvercharges()
    {
        return timerTrigger - CrashLink.overchargeCount;
    }
}
