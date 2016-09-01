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

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    [ContextMenu("SpawnGrid")]
    public void SpawnGrid()
    {
        Debug.Log("SPAWNME");
        StartCoroutine("SpawnGridTimed", spawnTime);
    }

    IEnumerator SpawnGridTimed(float waitTime)
    {
        Vector3 spawnPos = startPoint.position;

        for (int i = 0; i < rowCount; i++)
        {
            for(int j = 0; j < colCount; j++)
            {
                //Debug.Log(i + ";" + j + " sldkfj");
                GameObject o = Instantiate(spawnObjects[spawnIndex], spawnPos, Quaternion.identity) as GameObject;
                o.SendMessage(spawnMessage[spawnIndex], SendMessageOptions.DontRequireReceiver);
                yield return new WaitForSeconds(waitTime);
                spawnPos.x += xSpace;
            }
            spawnPos.x = startPoint.position.x;
            spawnPos.y += ySpace;
        }

        
    }
}
