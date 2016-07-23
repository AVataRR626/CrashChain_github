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

[RequireComponent(typeof(TextMesh))]
public class MainVolumeDisplay : MonoBehaviour 
{
	private TextMesh txt;

	// Use this for initialization
	void Start () 
	{
		txt = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		txt.text = AudioListener.volume.ToString("0.00");
	}
}
