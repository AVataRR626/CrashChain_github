/*
QSI Utils
Rotates object towards direction of travel...

Matt Cabanag
31 Oct 2016
*/

using UnityEngine;
using System.Collections;

public class DirectionRotate : MonoBehaviour
{
    public Rigidbody2D reference;

	// Use this for initialization
	void Start ()
    {
        if (reference == null)
            reference = GetComponent<Rigidbody2D>();

        if (reference == null)
            reference = transform.parent.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(reference != null)
        { 
            QuadrantMode();
        }
    }

    void QuadrantMode()
    {
        if (reference.velocity.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 270);

        if(reference.velocity.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 90);

        if (reference.velocity.y > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (reference.velocity.y < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
