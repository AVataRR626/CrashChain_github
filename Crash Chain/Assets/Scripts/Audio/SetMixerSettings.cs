using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetMixerSettings : MonoBehaviour {

	public AudioMixerSnapshot set;
	public float fade = 1.0f;

	// Use this for initialization
	void Start () {

		set.TransitionTo (fade);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
