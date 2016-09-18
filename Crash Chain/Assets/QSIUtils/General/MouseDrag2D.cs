using UnityEngine;
using System.Collections;

public class MouseDrag2D : MonoBehaviour
{
    public bool disableTracking = false;
    public bool verticalBlock = false;
    public bool horizontalBlock = false;

    public Vector3 startingPos;

    private Vector3 offset;

    private Rigidbody2D rb2d;
    private bool dragMode = false;
    private bool hadRigidbody = false;

    public bool DragMode
    {
        get
        {
            return dragMode;
        }
    }

    // Use this for initialization
    void Start ()
    {
        startingPos = transform.position;

        rb2d = GetComponent<Rigidbody2D>();

        hadRigidbody = (rb2d != null);
	}

    void Update()
    {
        if (dragMode && !disableTracking)
            TrackMouse();

        if (Input.GetMouseButtonUp(0))
        {
            dragMode = false;

            if(CameraClickMove.Instance != null)
                CameraClickMove.Instance.pauseMove = false;
        }
    }

    public void PauseTracking(float time)
    {
        disableTracking = true;
        dragMode = false;
        Invoke("EnableTracking", time);
    }

    public void EnableTracking()
    {
        disableTracking = false;
    }

    public void PauseTrackingHorizontal()
    {
        horizontalBlock = true;
    }

    public void PauseTrackingHorizontal(float time)
    {
        horizontalBlock = true;
        Invoke("EnableTrackingHorizontal", time);
    }

    public void EnableTrackingHorizontal()
    {
        horizontalBlock = false;
    }

    public void PauseTrackingVertical()
    {
        verticalBlock = true;
    }

    public void PauseTrackingVertical(float time)
    {   
        verticalBlock = true;
        Invoke("EnableTrackingVertical", time);
    }

    public void EnableTrackingVertical()
    {
        verticalBlock = false;
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

        if(CameraClickMove.Instance != null)
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

        if(CameraClickMove.Instance !=  null)
            CameraClickMove.Instance.pauseMove = false;


        //Debug.Log("MouseDrag2D:OnMouseUp");
    }

    public void TrackMouse()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (verticalBlock)
            newPos.y = transform.position.y + offset.y;
        
        if (horizontalBlock)
            newPos.x = transform.position.x + offset.x;

        newPos -= offset;
        newPos.z = startingPos.z;

        

        transform.position = newPos;

        //Debug.Log("MouseDrag2D: TrackMouse(): " + newPos + " | " + Input.mousePosition + " | " + offset);
    }


}
