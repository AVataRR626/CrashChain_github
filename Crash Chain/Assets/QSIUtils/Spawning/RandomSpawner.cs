//Quantum Shade Interactive - General Use
//RandomSpawner.cs
//
//Randomly spawns stuff from a given list.
//
//by:
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayRandomSound))]

public class RandomSpawner : MonoBehaviour 
{

	public GameObject [] spawnList;
	public Vector3 spawnRange = new Vector3(5,5,0.5f);
	public Vector3 snap = new Vector3(5,5,5);
	public int spawnCount = 1;
	public float spawnInterval = 10;
	public bool continuousSpawn = true;
	public bool spawnOnStart = false;

	public float clock = 0;
	private PlayRandomSound rndSoundPlayer;

	// Use this for initialization
	void Start () 
	{
		rndSoundPlayer = GetComponent<PlayRandomSound>();

		if(spawnOnStart)
			Spawn ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(continuousSpawn)
		{
			if(clock <= 0)
			{
				Spawn ();
				clock = spawnInterval;
			}
			else
			{
				clock -= Time.deltaTime;
			}
		}
	
	}

	public GameObject Spawn()
	{
		float snapX = (int)((int)Random.Range(-spawnRange.x,spawnRange.x)/snap.x)*snap.x;
		float snapY = (int)((int)Random.Range(-spawnRange.y,spawnRange.y)/snap.y)*snap.y;
		float snapZ = (int)((int)Random.Range(-spawnRange.z,spawnRange.z)/snap.z)*snap.z;
		
		
		Vector3 spawnOffset = new Vector3(snapX,snapY,snapZ);
		
		GameObject newSpawn = (GameObject) Instantiate (spawnList[(int)Random.Range (0,spawnList.Length)],transform.position + spawnOffset,transform.rotation);

		rndSoundPlayer.PlayRandSound();

		return newSpawn;

	}
}
