//Quantum Shade Interactive - General Use
//
//ConstantRotate.cs
//Keep rotating!
//
//by:
//1) Matt Cabanag
//2) ...

using UnityEngine;
using System.Collections;

public class ConstantRotate : MonoBehaviour {

	public Vector3 rotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (rotation*Time.deltaTime);
	}
}
