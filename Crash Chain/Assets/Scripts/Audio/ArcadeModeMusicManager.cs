using UnityEngine;
using System.Collections;

public class ArcadeModeMusicManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ArcadeModeMusicManager [] music_managers = FindObjectsOfType(typeof(ArcadeModeMusicManager)) as ArcadeModeMusicManager[];

		if(music_managers.Length <= 1)
		{
			DontDestroyOnLoad(gameObject);

			foreach(Transform child in transform)
			{
				DontDestroyOnLoad(child.gameObject);
			}
		}
		else
		{
			foreach(Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			Destroy(gameObject);
		}


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
