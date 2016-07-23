//General QSI Utility
//
//Music Manager.cs
//-
//
//by 
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class MusicManager : MonoBehaviour 
{
	public AudioClip introSequence;
	public AudioClip mainSequence;
	public bool persistent = false;
	
	private bool mainPlaySwitch = false;

	// Use this for initialization
	void Start () 
	{
		MusicManager [] music_managers = FindObjectsOfType(typeof(MusicManager)) as MusicManager[];
		
		if(music_managers.Length <= 1)
		{
		
			if(introSequence != null)
			{
				GetComponent<AudioSource>().clip = introSequence;
				GetComponent<AudioSource>().Play();
			}
			
			if(persistent)
			{
				DontDestroyOnLoad(gameObject);
				
				foreach(Transform child in transform)
				{
					DontDestroyOnLoad(child.gameObject);
				}
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
	void FixedUpdate () 
	{
		if(!GetComponent<AudioSource>().isPlaying)
		{
			mainPlaySwitch = true;
		}

		if(mainPlaySwitch && !GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().clip = mainSequence;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();

		}
	}
}
