//Marsfield RTS Framework
//July 2014 revision
//Matt Cabanag  

using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour 
{
	
	public GameObject spawnObject;
	public int spawnCount = 1;
	
	public bool spawnSwitch = true;
	public float spreadCoefficient = 1.0f; 
	public float spawnClock = 0.0f;
	public float spawnInterval = 0.0f;
	public bool oneShot = false;
	public int randomClockOffset = 0;
	public int randomIntervalOffset = 0;
	
	// Use this for initialization
	void Start () 
	{
		spawnClock += Random.Range(0,randomClockOffset);
		spawnInterval += Random.Range(0,randomIntervalOffset);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(spawnClock <= 0)
		{
			if(spawnSwitch)
			{
				spawnSwitch = false;
				SpawnObjects();
				
			}
	
		}
		else
		{
			spawnClock -=Time.deltaTime;
		}
	
	}
	
	public void SpawnObjects()
	{
		
		if(spawnInterval > 0)
		{
			spawnClock = spawnInterval;
			spawnSwitch = true;
		}
		
		if(spawnObject != null)
		{
			for(int i = 0; i < spawnCount; i++)
			{
				Vector3 newPost = new Vector3
								((float)UnityEngine.Random.Range(-spawnCount*spreadCoefficient,spawnCount*spreadCoefficient),
									transform.position.y,
									UnityEngine.Random.Range(-spawnCount*spreadCoefficient,spawnCount*spreadCoefficient)
								);
				GameObject newObject = (GameObject)Instantiate(spawnObject,transform.position + newPost,transform.rotation);
			}
		}

		if(oneShot)
			Destroy (this);
		
	}
}
