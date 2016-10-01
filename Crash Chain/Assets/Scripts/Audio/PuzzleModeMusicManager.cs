using UnityEngine;
using System.Collections;

public class PuzzleModeMusicManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

		PuzzleModeMusicManager [] music_managers = FindObjectsOfType(typeof(PuzzleModeMusicManager)) as PuzzleModeMusicManager[];

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
