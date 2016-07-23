//Quantum Shade Interactive - General Use
//PlayRandomSound.cs
//
//Plays a random sound from a given list
//
//by:
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class PlayRandomSound : MonoBehaviour 
{
	public bool playOnSpawn = true;
	public AudioClip [] sounds;

	// Use this for initialization
	void Start () 
	{
		if(playOnSpawn)
			PlayRandSound();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void PlayRandSound()
	{
		if(sounds.Length > 0)
		{
			int soundIndex = Random.Range(0,sounds.Length-1);
			//Debug.Log(soundIndex + ";" + sounds.Length);
			
			if(sounds[soundIndex] != null)
			{
				GetComponent<AudioSource>().clip = sounds[soundIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
