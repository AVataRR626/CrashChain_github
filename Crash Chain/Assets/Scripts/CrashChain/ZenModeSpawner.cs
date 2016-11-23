/*
 * Crash Chain stuff.

 Matt Cabanag, Nov 2016
 */

using UnityEngine;
using System.Collections;

public class ZenModeSpawner : MonoBehaviour
{
    public static ZenModeSpawner instance;
    public static bool spawnMode = true;

    public GameObject crashPhaseTree;
    public GameObject placementPhaseTree;
    public int spawnAmmo = 5;
    public int level = 0;
    public int spawnIndex = 0;
    public int [] levelThreshold;
    public int[] spawnCounts;
    public CrashLink [] spawnObjects;
    public string [] spawnMessages;
    
    [Header("Spawn Finished Triggers")]
    public GameObject [] spawnFinishedObjects;
    public string [] spawnFinishedMessages;
    public GameObject[] spawnFinishedActivateList;

    [Header("Level Up Triggers")]
    public GameObject[] levelUpObjects;
    public string [] levelUpMessages;
    public GameObject[] levelUpDeactivateList;

    float originalZ;

    bool finishedTrigger = false;
    SmoothSnap snapper;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        originalZ = transform.position.z;
        snapper = GetComponent<SmoothSnap>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        ManageClicks();

    }

    void ManageClicks()
    {
        if (Input.GetMouseButtonDown(0) && spawnMode)
        {
            Vector3 newPos;

            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = originalZ;

            transform.position = newPos;

            if (spawnAmmo > 0)
            {
                Spawn();                
                spawnAmmo--;
            }

            snapper.SetAnchorGridCoordinatesOnPos();


            if (spawnAmmo <= 0)
            {
                if (!finishedTrigger)
                {
                    SpawnFinished();
                    finishedTrigger = true;
                }
            }

            
        }


        if(spawnAmmo > 0)
        { 
            if (!placementPhaseTree.activeSelf)
                placementPhaseTree.SetActive(true);

            if (crashPhaseTree.activeSelf)
            {
                crashPhaseTree.SendMessage("Reset");
                crashPhaseTree.SetActive(false);
            }
        }
        else
        {
            if (placementPhaseTree.activeSelf)
            {
                placementPhaseTree.SendMessage("Reset");
                placementPhaseTree.SetActive(false);
            }

            if (!crashPhaseTree.activeSelf)
                crashPhaseTree.SetActive(true);
        }

    }

    void Spawn()
    {
        int randomisedSI = Random.Range(0, spawnIndex+1);

        CrashLink c = Instantiate(spawnObjects[randomisedSI], transform.position, Quaternion.identity) as CrashLink;
        c.SendMessage(spawnMessages[spawnIndex], SendMessageOptions.DontRequireReceiver);
    }

    void SpawnFinished()
    {
        GenUtils.SendMessages(ref spawnFinishedObjects, ref spawnFinishedMessages);
        GenUtils.SetActiveObjects(ref spawnFinishedActivateList, true);
    }

    void LevelUp()
    {
        level++;
        finishedTrigger = false;

        for (int i = levelThreshold.Length - 1; i > 0; i--)
        {
            if (level >= levelThreshold[i])
            {
                spawnIndex = i;
                break;
            }
        }

        spawnAmmo = spawnCounts[spawnIndex];

        GenUtils.SetActiveObjects(ref levelUpObjects, true);
        GenUtils.SendMessages(ref levelUpObjects, ref levelUpMessages);
        GenUtils.SetActiveObjects(ref levelUpDeactivateList, false);

        spawnMode = true;
    }
}
