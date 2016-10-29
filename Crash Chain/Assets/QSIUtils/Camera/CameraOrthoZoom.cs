using UnityEngine;
using System.Collections;

/*BounceBlaster
CameraOrthoZoom.cs

Fakes the zoom-in/out effect for an orthographic camera.

Authors:
1) Matt Cabanag
2) ..
3) ..

*/

public class CameraOrthoZoom : MonoBehaviour 
{

    public static float prevSceneZoom;
	
	public float minZoom = 60;
	public float maxZoom = 100;
	public float zoomMouseCoefficient = 1.25f;
	public float zoomTouchCoefficient = 0.75f;
    public bool focusZoom = false;
    public float focusZoomRate = 0.2f;
    public float focusZoomFactor = 0.5f;    
    //public float yPos = 100f;



    public TextMesh debug;

	private float touchDistance;
	private float prevTouchDistance = 0;
    private float prevTouchDelta = 0;
    private float magDelta;
	private float originalZoom;

    private CameraClickMove camMover;
    private CameraOrthoLerp camAutoZoomer;

    // Use this for initialization
    void Start () 
	{
		//Vector3 startPos =  new Vector3(transform.position.x,yPos,transform.position.z);
		//transform.position = startPos;

		originalZoom = Camera.main.orthographicSize;
        camAutoZoomer = GetComponent<CameraOrthoLerp>();

        if(camAutoZoomer != null && prevSceneZoom != 0)
        {
            camAutoZoomer.destinationZoom = originalZoom;
            camAutoZoomer.sourceZoom = prevSceneZoom;
            camAutoZoomer.moveSwitch = true;
        }

        if (camMover == null)
            camMover = FindObjectOfType<CameraClickMove>();

        if (Input.touchCount >= 2)
        {


            Destroy(GetComponent<IntroOrthoCamZoom>());

            Vector2 touch0, touch1;

            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;

            touchDistance = Vector2.Distance(touch0, touch1);
        }

    }

    void OnDisable()
    {
        prevSceneZoom = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update () 
	{
		if (Input.touchCount >= 2)
		{
            Destroy(GetComponent<IntroOrthoCamZoom>());

			Vector2 touch0, touch1;

			touch0 = Input.GetTouch(0).position;
			touch1 = Input.GetTouch(1).position;
			
			touchDistance = Vector2.Distance(touch0, touch1);

            //start from zero if you've just entered touch phase!
            if (Input.touches[1].phase == TouchPhase.Began)
            {
                prevTouchDistance = touchDistance;
            }

            magDelta = (touchDistance - prevTouchDistance); 

            if (debug != null)				
				debug.text = prevTouchDistance.ToString();

            //float touchZoom = 0;
            //touchZoom = (touchDistance-prevTouchDistance) * zoomMouseCoefficient * Time.deltaTime;
            //Camera.main.orthographicSize += Mathf.Min(Mathf.Max(Camera.main.orthographicSize - (touchZoom), minZoom), maxZoom);

            if(magDelta != 0)
                Camera.main.orthographicSize -= magDelta * zoomTouchCoefficient * Time.deltaTime;

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

            prevTouchDistance = touchDistance;
            prevTouchDelta = magDelta;

        }
		else if (Input.GetAxis("Mouse ScrollWheel") != 0) 
		{
			Destroy(GetComponent<IntroOrthoCamZoom>());

			Camera.main.orthographicSize = Mathf.Min(Mathf.Max(Camera.main.orthographicSize - (Input.GetAxis("Mouse ScrollWheel") * zoomTouchCoefficient), minZoom), maxZoom);
		}
		else if (focusZoom)
		{

			prevTouchDistance = 0;

			if(debug != null)				
				debug.text = prevTouchDistance.ToString();

            float zoomRef = camMover.GetZoomDistanceReference();
            if (zoomRef > 0)
            {
                float currentZoom = Camera.main.orthographicSize;
                float goalZoom = zoomRef * focusZoomFactor;

                if (goalZoom > maxZoom)
                    goalZoom = maxZoom;

                Camera.main.orthographicSize = Mathf.Lerp(currentZoom, goalZoom, focusZoomRate * Time.deltaTime);
            }

		}

        if (debug != null)
            debug.text = "magDelta: " + magDelta.ToString();

    }
}
