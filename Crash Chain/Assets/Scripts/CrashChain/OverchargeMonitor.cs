using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OverchargeMonitor : MonoBehaviour
{
    public static OverchargeMonitor instance;
    public static string crashBoltTag = "CrashBolt";

    [Header("Trigger Settings")]
    public float timeLimit = 1.65f;    
    public int timerTrigger = 1;    
    public CrashChainDynLevelSaver levelSaver;
    public PopulationCheck popChecker;
    
    public GameObject [] activateObjects;
    public GameObject [] disableObjects;
    public GameObject [] messageList;
    public string message;
    public int overchargeLimInc = 0;

    [Header("Warning Settings")]
    public int warningTrigger = 2;
    public GameObject [] warningObjects;    
    public string [] warningMessages;
    public GameObject[] warningDisableList;

    private int moveCount = 0;
    private float clock;
    private bool saveSwitch = false;
    private bool warningSwitch = false;

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

    public int GetCrashBoltCount()
    {
        GameObject[] crashBolts = GameObject.FindGameObjectsWithTag(crashBoltTag);

        return crashBolts.Length;
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

                if(GetCrashBoltCount() <= 0)
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

        ManageWarnings();
    }

    public void ManageWarnings()
    {
        if(RemainingOvercharges() <= warningTrigger)
        {
            int i = 0;
            if(!warningSwitch)
            {
                foreach(GameObject o in warningObjects)
                {
                    o.SetActive(true);
                    o.SendMessage(warningMessages[i],SendMessageOptions.DontRequireReceiver);
                    i++;
                }

                foreach(GameObject o in warningDisableList)
                {
                    o.SetActive(false);
                }
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
            o.SendMessage(message, SendMessageOptions.DontRequireReceiver);

        //increment retry count, player has lost...
        levelSaver.ReryIncrement();

        //ResetOvercharge();
    }

    public void ResetOvercharge()
    {
        //Debug.Log("--------------RESETTING OVERCHARGE");


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
        int result = timerTrigger - CrashLink.overchargeCount;

        if(result > 0)
            return result;

        return 0;
    }
}
