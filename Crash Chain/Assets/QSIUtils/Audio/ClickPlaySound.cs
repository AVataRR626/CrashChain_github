//General QSI Utility
//
//ClickPlaySound.cs
//-
//
//by 
//1) Matt Cabanag
//2) ...
//3) ...

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class ClickPlaySound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		GetComponent<AudioSource>().Play ();
	}
}
