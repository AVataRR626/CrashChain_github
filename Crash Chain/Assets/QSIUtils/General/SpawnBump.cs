//Quantum Shade Interactive - General Use
//
//SpawnBump.cs
//Spawn stuff whenever you hit stuff
//
//by:
//1) Matt Cabanag
//2) ...

using UnityEngine;
using System.Collections;

public class SpawnBump : MonoBehaviour 
{
	public GameObject [] spawnList;
	public string [] tagFilter;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(GetComponent<Collider2D>() != null)
			if(GetComponent<Collider2D>().isTrigger)
				return;

		bool filterPassed = true;
		
		if(tagFilter.Length > 0)
		{
			filterPassed = false;
			
			foreach(string f in tagFilter)
			{
				if(f == other.gameObject.tag)
					filterPassed = true;
			}
		}
		
		if(!filterPassed)
			return;

		Spawn();

	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(GetComponent<Collider2D>() != null)
			if(!GetComponent<Collider2D>().isTrigger)
				return;

		bool filterPassed = true;
		
		if(tagFilter.Length > 0)
		{
			filterPassed = false;
			
			foreach(string f in tagFilter)
			{
				if(f == other.gameObject.tag)
					filterPassed = true;
			}
		}
		
		if(!filterPassed)
			return;

		Spawn();
	}

	void Spawn()
	{
		foreach(GameObject ds in spawnList)
		{
			GameObject d = (GameObject)Instantiate(ds,transform.position,transform.rotation);
			d.tag = tag;
		}
	}
}
