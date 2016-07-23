using UnityEngine;
using System.Collections;

public class KeySpawner : MonoBehaviour 
{
	public KeyCode [] spawnKeys;
	public GameObject [] spawnItems;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < spawnKeys.Length; i++)
		{
			if(Input.GetKeyDown(spawnKeys[i]))
			{
				SpawnItem(i);
			}
		}
	}

	public GameObject SpawnItem(int i)
	{
		return Instantiate(spawnItems[i], transform.position, transform.rotation) as GameObject;

	}
}
