using UnityEngine;
using System.Collections;

public class GridSpawner : MonoBehaviour
{
    public Transform startPoint;
    public int colCount = 3;
    public int rowCount = 4;
    public float xSpace = 45;
    public float ySpace = 45;
    public float spawnTime = 0.25f;
    public GameObject[] spawnObjects;
    public string [] spawnMessage;
    public int spawnIndex = 0;
    public int maxSpawnIndex = 0;
    public bool randomiseSpawnIndex = true;
    public bool spawnOnStart = true;
    public int spawnCount = 0;
    public bool randomPos = false;

    // Use this for initialization
    void Start ()
    {
	    if(spawnOnStart)
        {
            StartCoroutine("SpawnGridTimed", spawnTime);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    [ContextMenu("SpawnGrid")]
    public void SpawnGrid()
    {
        //Debug.Log("SPAWNME");
        StartCoroutine("SpawnGridTimed", spawnTime);
    }

    IEnumerator SpawnGridTimed(float waitTime)
    {
        Vector3 spawnPos = startPoint.position;

        spawnCount = 0;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {

                if(randomPos)
                {
                    spawnPos.x = startPoint.position.x + xSpace * Random.Range(0, colCount);
                    spawnPos.y = startPoint.position.x + ySpace * Random.Range(0, rowCount);
                }

                if (randomiseSpawnIndex)
                {
                    spawnIndex = Random.Range(0, maxSpawnIndex + 1);
                }

                GameObject o = Instantiate(spawnObjects[spawnIndex], spawnPos, Quaternion.identity) as GameObject;
                o.SendMessage(spawnMessage[spawnIndex], SendMessageOptions.DontRequireReceiver);

                if (!randomPos)
                    spawnPos.x += xSpace;

                spawnCount++;
                //Debug.Log("SpawnCount:" + spawnCount + " ;" + i + ";" + j);

                yield return new WaitForSeconds(waitTime);
            }
            
            if(!randomPos)
            { 
                spawnPos.x = startPoint.position.x;
                spawnPos.y += ySpace;
            }
            else
            {
                spawnPos = startPoint.position;
            }
        }
        
    }
}
