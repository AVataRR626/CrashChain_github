//Hectic Game Jam 
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class QSIKillable : MonoBehaviour 
{
	public int maxHealth = 10;
	public int health = 10;

	public GameObject [] deathSpawnList;

	private float dmgAccumulator;
	private float hlthAccumulator;
	private QSICharConn qsiCharCon;


	// Use this for initialization
	void Start () 
	{
		qsiCharCon = GetComponent<QSICharConn>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(health > maxHealth)
			health = maxHealth;

		if(health <= 0)
		{
			SpawnDeathList();
			Destroy (gameObject);
		}
	}

	void SpawnDeathList()
	{
		for(int i = 0; i < deathSpawnList.Length; i++)
		{
			Instantiate(deathSpawnList[i],transform.position,transform.rotation);
		}
	}

	/*
	public void Damage(int dmg)
	{
		Debug.Log ("IntDmg");
		health -= dmg;
	}*/

	public void Damage(float dmg)
	{

		//Debug.Log ("FloatDmg:" + dmg);
		dmg += dmgAccumulator;

		health -= (int)dmg;

		dmgAccumulator = dmg - (int)dmg;
	}

	public void AddHealth(float hlth)
	{
		hlth += hlthAccumulator;

		health +=(int)hlth;

		hlthAccumulator = hlth - (int)hlth;
	}
}
