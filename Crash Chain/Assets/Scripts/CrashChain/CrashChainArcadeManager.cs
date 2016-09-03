using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GridSpawner))]
public class CrashChainArcadeManager : MonoBehaviour
{
    public int level = 0;
    public int [] levelThreshold;
    public int [] maxSpawnIndex;
    public Vector2 [] levelDimensions;
    public Vector3 [] startPointPositions;

    public static CrashChainArcadeManager instance;

    GridSpawner mySpawner;

	// Use this for initialization
	void Start ()
    {
        mySpawner = GetComponent<GridSpawner>();
        instance = this;
        CrashLink.overchargeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LevelUp()
    {
        level++;

        for(int i = levelThreshold.Length - 1; i > 0; i--)
        {
            if(level >= levelThreshold[i])
            {
                mySpawner.maxSpawnIndex = maxSpawnIndex[i];
                mySpawner.colCount = (int)levelDimensions[i].x;
                mySpawner.rowCount = (int)levelDimensions[i].y;
                mySpawner.startPoint.localPosition = startPointPositions[i];
                break;
            }
        }
    }
}
