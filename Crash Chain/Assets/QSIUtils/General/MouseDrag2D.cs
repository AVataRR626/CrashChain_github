using UnityEngine;
using System.Collections;

public class MouseDrag2D : MonoBehaviour
{
    public static Vector3 MousePosition;

    [Header("Tracking Parameters")]
    public bool disableTracking = false;
    public bool verticalBlock = false;
    public bool horizontalBlock = false;
    public Vector3 startingPos;
    private Vector3 offset;

    [Header("Travel Limits")]
    public float maxY =1000;
    public float minY = -1000;
    public float maxX = 1000;
    public float minX = -1000;

    private Rigidbody2D rb2d;
    private bool dragMode = false;
    private bool hasRigidBody = false;

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

        hasRigidBody = (rb2d != null);
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

        EnforceLimits();
    }

    public void EnforceLimits()
    {
        Vector3 pos = transform.position;

        if (pos.x > maxX)
            pos.x = maxX;

        if (pos.x < minX)
            pos.x = minX;

        if (pos.y > maxY)
            pos.y = maxY;

        if (pos.y < minY)
            pos.y = minY;

        transform.position = pos;
        //CalculateOffset();
    }

    public void PauseTracking(float time)
    {
        OnMouseUp();
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

    public void CalculateOffset()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = newPos - transform.position;
    }

    public void DisableDrag()
    {
        dragMode = false;
    }

    public void StartDrag()
    {

        CalculateOffset();

        if (hasRigidBody)
        {
            if (rb2d != null)
            {
                //rb2d.isKinematic = true;
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
        if(hasRigidBody)
        { 
            if (rb2d != null)
            {
                rb2d.isKinematic = false;

            }
        }

        dragMode = false;

        if(CameraClickMove.Instance !=  null)
            CameraClickMove.Instance.pauseMove = false;

    }

    public void TrackMouse()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (verticalBlock)
            MousePosition.y = transform.position.y + offset.y;

        if (horizontalBlock)
            MousePosition.x = transform.position.x + offset.x;

        MousePosition -= offset;
        MousePosition.z = startingPos.z;

        transform.position = MousePosition;
    }
}
