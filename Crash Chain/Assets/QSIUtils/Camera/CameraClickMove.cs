using UnityEngine;
using System.Collections;

/*BounceBlaster Project
CameraClickMove.cs

Matt's basic way of click-panning.

Authors:
1) Matt Cabanag
2) ..
3) ..

*/

public class CameraClickMove : MonoBehaviour 
{
    public static CameraClickMove Instance;

	public LayerMask qBrickMask = 8;
	public bool pauseMove = false;
	public float moveFactor = 1.25f;
	public float leftBorder = -3;
	public float rightBorder = 3;
	public float topBorder = -3;
	public float bottomBorder =3;
    public int touchCountTrigger = 2;
	public KeyCode moveKey = KeyCode.Mouse1;
	public KeyCode cancelMoveKey = KeyCode.Z;
	public float maxForceMoveDelta = 0.5f;
    public Transform [] focusList = new Transform[100];
    public float focusFactor = 0.25f;
    public Vector3 targetPos;
    public float maxDelta = 0.5f;

    public TextMesh debug;

	
	private bool moveMode = false;
	private Vector3 mouseDown = Vector3.zero;
	private Vector3 currentMousePos = Vector3.zero;
    private Vector3 prevMousePos = Vector3.zero;
    private Vector3 focalPoint;
    private int focusCount;
	
	// Use this for initialization
	void Start () 
	{
        Instance = this;
        focusList = new Transform[100];

        Invoke("Init", 0.1f);
       
    }

    void Init()
    {
        targetPos = transform.position;
        mouseDown = transform.localPosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentMousePos = transform.localPosition;
        prevMousePos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (!Input.GetKey(cancelMoveKey))
        {
            if (Input.touchCount > touchCountTrigger -1)
            {
                
                HandleTouch();

            }            
            else if (Input.GetKey(moveKey))
            {
                HandleMouse();
            }

        }

        //disable movement after every button release
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetKeyUp(moveKey))
		{

            moveMode = false;
            NullifyFocusList();
        }

        
        FocusMove();

        if (debug != null)
        {
            debug.text = Input.touchCount.ToString() + " TOUCH: " +
                mouseDown.x.ToString("0.0000") +
                " | mouseLoc: " +
                currentMousePos.x.ToString("0.0000") +
                " | delta: " +
                (mouseDown - currentMousePos).magnitude.ToString("0.0000");

            for(int i = 0; i < Input.touchCount; i++)
            {
                debug.text += "\n";
                debug.text += "Touch #" + i + " : " + Input.GetTouch(i).position.ToString() + "\n";
            }
        }


    }

    void HandleTouch()
    {
        if (Input.GetTouch(touchCountTrigger-1).phase == TouchPhase.Began)
        {
            mouseDown = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            currentMousePos = mouseDown;
            NullifyFocusList();
        }

        if (Input.GetTouch(touchCountTrigger - 1).phase == TouchPhase.Moved)
        {
            moveMode = !pauseMove;

            currentMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector3 delta = (mouseDown - currentMousePos);

            MoveCam(delta);
        }

        if(Input.GetTouch(touchCountTrigger).phase == TouchPhase.Ended)
        {
            //currentMousePos = mouseDown;
            mouseDown = currentMousePos;
        }
    }

    void HandleMouse()
    {
        if(Input.GetKeyDown(moveKey))
        {

            mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos = mouseDown;
            prevMousePos = mouseDown;
            NullifyFocusList();

        }

        if (Input.GetKey(moveKey))
        {

            moveMode = !pauseMove;


            currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            Vector3 delta = (mouseDown - currentMousePos);
            //Vector3 delta = (prevMousePos - currentMousePos);

            MoveCam(delta);

            prevMousePos = currentMousePos;
        }

    }


    //focus on the centre!
    void FocusMove()
    {
        if(focusList != null)
        {
            if(focusList.Length > 0)
            { 
                focalPoint = Vector3.zero;
                focusCount = 0;

                //Get the average point of all the objects in the focus list.
                //That is the focal point.
                foreach (Transform t in focusList)
                {
                    //only consider non null ones
                    if (t != null)
                    {
                        focalPoint += t.position;
                        focusCount++;
                    }
                }


                //go focus!
                if(focusCount > 0)
                {
                    focalPoint = focalPoint / focusCount;

                    //nullify z differences
                    focalPoint.z = transform.position.z;

                    MoveTo(focalPoint, focusFactor * Time.deltaTime);
                }
            }
        }

        //MoveTo(focalPoint, focusFactor * Time.deltaTime);
    }

    //place this focal item in the first empty spot found.
    public void AddFocusItem(Transform t)
    {
        //don't add if item is already in there!        
        if (isInFocusList(t))
            return;

        for (int i = 0; i < focusList.Length; i++)
        {
            if(focusList[i] == null)
            {   
                focusList[i] = t;
                i = focusList.Length;
            }
        }
    }

    public bool isInFocusList(Transform t)
    {
        for (int i = 0; i < focusList.Length; i++)
        {
            if (focusList[i] == null)
            {
                if (focusList[i] == t)
                    return true;
            }
        }

        return false;
    }

    //nullify the focus list
    public void NullifyFocusList()
    {
        for(int i = 0; i < focusList.Length; i++)
        {
            focusList[i] = null;
        }
    }

    public float GetZoomDistanceReference()
    {
        float furthestDistance = 0;

        for (int i = 0; i < focusList.Length; i++)
        {
            if(focusList[i] != null)
            { 
                float dist = Vector3.Distance(focusList[i].position, transform.position);

                if (dist > furthestDistance)
                    furthestDistance = dist;
            }
        }

        return furthestDistance;
    }

    public void MoveTo(Vector3 newPos, float rate)
    {

        Vector3 delta = newPos - transform.position;
        delta *= rate;

        ForceMoveCam(delta);
    }

    void MoveCam(Vector3 delta)
    {
        MoveCam(delta, moveFactor);
    }


	void MoveCam(Vector3 delta, float mFactor)
	{

        if (moveMode)
        {
            //transform.position += delta* mFactor;
            if (delta.magnitude < maxDelta)
                targetPos += delta * mFactor;
        }

        transform.position = Vector3.Lerp(targetPos, transform.position, 0.02f * Time.deltaTime);
    }

    void BorderLimit()
    {
        if (transform.position.x < leftBorder)
            transform.position = new Vector3(leftBorder, transform.position.y, transform.position.z);

        if (transform.position.x > rightBorder)
            transform.position = new Vector3(rightBorder, transform.position.y, transform.position.z);

        if (transform.position.y < topBorder)
            transform.position = new Vector3(transform.position.x, topBorder, transform.position.z);

        if (transform.position.y > bottomBorder)
            transform.position = new Vector3(transform.position.x, bottomBorder, transform.position.z);
    }

    public void ForceMoveCam(Vector3 delta, float mFactor)
    {
        bool origMode = moveMode;
        moveMode = true;

        if (delta.magnitude > maxForceMoveDelta)
        {
            delta.Normalize();
            delta *= maxForceMoveDelta;
        }

        MoveCam(delta, mFactor);

        moveMode = origMode;
    }

    public void ForceMoveCam(Vector3 delta)
	{
        bool origMode = moveMode;
		moveMode = true;

		if(delta.magnitude > maxForceMoveDelta)
		{
			delta.Normalize();
			delta *= maxForceMoveDelta;
		}

		MoveCam(delta);

        moveMode = origMode;
	}

    public void ResetMouseData()
    {
        mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prevMousePos = mouseDown;
        currentMousePos = mouseDown;
    }
}
