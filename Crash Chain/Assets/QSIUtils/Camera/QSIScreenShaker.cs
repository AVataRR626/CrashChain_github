/*
QSI General Utils
QSIScreenShaker - Needs to use PerlinShake component to shake the screen.
Attatch to objects that are meant to start screen shakes. i.e. Explosions.

-Matt Cabanag
2 Oct 2016
*/

using UnityEngine;
using System.Collections;

public class QSIScreenShaker : MonoBehaviour
{

	// Use this for initialization
	void Start () {

        Debug.Log("Boop:" + gameObject.name);
        PerlinShake p = Camera.main.GetComponent<PerlinShake>();
        p.StartShake();
	}
}
