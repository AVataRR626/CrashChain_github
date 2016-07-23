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
public class MusicVolumeDisplay : MonoBehaviour 
{
	private TextMesh txt;
	private MusicManager mscMgr;

	// Use this for initialization
	void Start () 
	{
		txt = GetComponent<TextMesh>();
		mscMgr = FindObjectOfType<MusicManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mscMgr != null)
			txt.text = mscMgr.GetComponent<AudioSource>().volume.ToString("0.00");
	}
}
