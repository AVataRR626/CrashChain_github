using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraOrthoLerp : MonoBehaviour
{
    public float sourceZoom;
    public float destinationZoom;
    public float lerpTime = 3;
    public bool startFix = true;
    public bool moveSwitch = false;


    public float lerpClock = 0;
    public float lerpValue = 0;
    public float zoomVal;

    private Camera cam;

    // Use this for initialization
    void Start ()
    {
        cam = GetComponent<Camera>();

        if(startFix)
        {
            sourceZoom = cam.orthographicSize;
            destinationZoom = cam.orthographicSize;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (moveSwitch)
            MoveToDestination();
	}

    void MoveToDestination()
    {
        if(destinationZoom != sourceZoom)
        {
            //move to your destination
            lerpClock += Time.deltaTime;

            lerpValue = lerpClock / lerpTime;

            if (lerpValue > 1)
            {
                lerpValue = 1;
                cam.orthographicSize = destinationZoom;
            }

            zoomVal = Mathf.Lerp(sourceZoom, destinationZoom, lerpValue);
            cam.orthographicSize = zoomVal;

            if (cam.orthographicSize == destinationZoom)
            {
                cam.orthographicSize = zoomVal;
                moveSwitch = false;
                lerpClock = 0;
                lerpValue = 0;
            }
        }
    }
}
