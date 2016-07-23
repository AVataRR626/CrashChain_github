using UnityEngine;
using System.Collections;

public class MouseDrag2D : MonoBehaviour
{
    private Vector3 startingPos;
    private Vector3 offset;

    private Rigidbody2D rb2d;
    private bool dragMode = false;
    private bool hadRigidbody = false;
	// Use this for initialization
	void Start ()
    {
        startingPos = transform.position;

        rb2d = GetComponent<Rigidbody2D>();

        hadRigidbody = (rb2d != null);
	}

    void Update()
    {
        if (dragMode)
            TrackMouse();

        if (Input.GetMouseButtonUp(0))
        {
            dragMode = false;
            CameraClickMove.Instance.pauseMove = false;
        }
    }

    public bool DragMode
    {
        get
        {
            return dragMode;
        }
    }

    public void StartDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = newPos - transform.position;


        if (hadRigidbody)
        {
            if (rb2d != null)
            {
                rb2d.isKinematic = true;
            }
        }

        dragMode = true;
        CameraClickMove.Instance.pauseMove = true;

    }

    void OnMouseDown()
    {
        StartDrag();

        TrackMouse();
    }

    /*
    void OnMouseDrag()
    {
        //TrackMouse();
    }
    */

    void OnMouseUp()
    {
        if(hadRigidbody)
        { 
            if (rb2d != null)
            {
                rb2d.isKinematic = false;

            }
        }

        dragMode = false;
        CameraClickMove.Instance.pauseMove = false;


        Debug.Log("MouseDrag2D:OnMouseUp");
    }

    public void TrackMouse()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos -= offset;
        newPos.z = startingPos.z;
        transform.position = newPos;

        //Debug.Log("MouseDrag2D: TrackMouse(): " + newPos + " | " + Input.mousePosition + " | " + offset);
    }


}
