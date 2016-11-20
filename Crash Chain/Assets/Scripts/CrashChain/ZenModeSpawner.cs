using UnityEngine;
using System.Collections;

public class ZenModeSpawner : MonoBehaviour
{
    public int level = 0;
    public int spawnIndex = 0;
    public CrashLink[] spawnObjects;
    public string[] spawnMessages;

    float originalZ;

	// Use this for initialization
	void Start ()
    {
        originalZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            Vector3 newPos;

            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = originalZ;

            transform.position = newPos;
        }
	}

    void Spawn()
    {
        CrashLink c = Instantiate(spawnObjects[spawnIndex], transform.position, Quaternion.identity) as CrashLink;
    }
}
