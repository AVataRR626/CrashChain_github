using UnityEngine;
using System.Collections;

public class LookAtMainCamera : MonoBehaviour {

	public Transform mainCam;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {

		transform.LookAt(mainCam.position);
	
	}
}
