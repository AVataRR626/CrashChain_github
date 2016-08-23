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
    public GameObject triggerObject;
    public string triggerMessage;
    public CrashChainDynLevelSaver levelSaver;

    private int pop;
    private GameObject[] tagSearch;

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
        tagSearch = GameObject.FindGameObjectsWithTag(checkTag);

        pop = tagSearch.Length;

        if(pop == triggerNum)
        {
            if(triggerObject != null)
            {
                triggerObject.SetActive(true);
                triggerObject.SendMessage(triggerMessage, SendMessageOptions.DontRequireReceiver);
                levelSaver.RegisterWin();
            }
        }
	}
}
