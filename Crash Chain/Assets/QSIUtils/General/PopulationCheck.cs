//General QSI Utility...
//Matt Cabanag..

using UnityEngine;
using System.Collections;

//Tracks the population count of a given tag,
//Triggers a specified game object when the trigger amount has been matched (exactly)
public class PopulationCheck : MonoBehaviour
{
    public string checkTag;
    public int triggerNum;
    public GameObject []  triggerObjects;
    public string triggerMessage;
    public CrashChainDynLevelSaver levelSaver;
    public float triggerDelay = 0.5f;
    public bool continuous = false;

    private int pop;
    private GameObject[] tagSearch;
    private bool triggerSwitch = false;

    float delayClock = 0.5f;

    // Use this for initialization
    void Start ()
    {
        if (levelSaver == null)
            levelSaver = FindObjectOfType<CrashChainDynLevelSaver>();
    }

    public int GetPop()
    {
        return pop;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (delayClock > 0)
            delayClock -= Time.deltaTime;
        else
            CheckPop();
	}

    void CheckPop()
    {
        tagSearch = GameObject.FindGameObjectsWithTag(checkTag);

        pop = tagSearch.Length;

        if (pop == triggerNum && !triggerSwitch)
        {
            Invoke("Trigger", triggerDelay);

            
            triggerSwitch = true;
            delayClock = triggerDelay;

        }
        else if(pop != triggerNum)
        {
            if (continuous)
            {
                triggerSwitch = false;
            }
        }
    }

    void Trigger()
    {
        if (triggerObjects != null)
        {
            foreach(GameObject t in triggerObjects)
            { 
                t.SetActive(true);
                t.SendMessage(triggerMessage, SendMessageOptions.DontRequireReceiver);
            }
            levelSaver.RegisterWin();
        }
    }
}
