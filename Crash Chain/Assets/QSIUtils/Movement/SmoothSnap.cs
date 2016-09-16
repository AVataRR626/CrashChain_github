using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LerpToPosition))]
public class SmoothSnap : MonoBehaviour
{
    public static SmoothSnap [,,] gridOccupation = new SmoothSnap[200,200,200];
    public static SmoothSnap[,,] desiredGridOccupation = new SmoothSnap[200, 200, 200];
    public static Vector3 nullGridCoords = new Vector3(-1, -1, -1);
    public static bool conflictLeftPush = true;//yield and go left...
    public static bool conflictDownPush = true;//yield and go down...
    public static bool focusConflictSwitch = false;

    LerpToPosition mover;
    public Vector3 snapSettings = new Vector3(1, 1, 1);
    public bool noSnapOverride = false;

    //should be private, but kept public for debugging...
    public bool snapSwitch = true;
    public bool occupancyCheck = false;
    public bool conflictSwitch = false;

    public Vector3 gridCoordinates;
    public Vector3 anchorGridCoordinates;

    Vector3 snapCoords;
    Vector3 occupiedGridSpot = new Vector3(-1,-1,-1);
    bool isFocus = false;

    Vector3 originalAnchor;

    // Use this for initialization
    void Start()
    {
        mover = GetComponent<LerpToPosition>();

        SetGridCoordinatesOnPos();
        anchorGridCoordinates = gridCoordinates;
        originalAnchor = anchorGridCoordinates;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) || noSnapOverride)
            snapSwitch = false;
        else
        {
            snapSwitch = true;            
        }

        if (snapSwitch)
        {
            //SetGridCoordinates();
            if (!isFocus)
            {
                if(!focusConflictSwitch)
                    ReturnToAnchorGridCoordintes();
            }
            else
            {
                SetGridCoordinatesOnPos();

                //handle conflict pushing handlers...
                conflictLeftPush = (anchorGridCoordinates.x > gridCoordinates.x);
                conflictDownPush = !(anchorGridCoordinates.y > gridCoordinates.y);
                
                anchorGridCoordinates = gridCoordinates;
                isFocus = false;
            }
        }


        if (transform.position != snapCoords && snapSwitch)
            Snap();

        mover.moveSwitch = snapSwitch;

    }

    public void SetGridCoordinatesOnPos()
    {
        gridCoordinates.x = Mathf.Round(transform.position.x / snapSettings.x);
        gridCoordinates.y = Mathf.Round(transform.position.y / snapSettings.y);
        gridCoordinates.z = Mathf.Round(transform.position.z / snapSettings.z);
    }

    public void SetAnchorGridCoordinatesOnPos()
    {
        SetGridCoordinatesOnPos();
        anchorGridCoordinates = gridCoordinates;
    }

    public void ReturnToAnchorGridCoordintes()
    {
        //Debug.Log("RETURN TO CONFLICG COORDS!");
        gridCoordinates = anchorGridCoordinates;
        conflictSwitch = false;
    }

    public void InstantSnap()
    {
        snapCoords.x = gridCoordinates.x * snapSettings.x;
        snapCoords.y = gridCoordinates.y * snapSettings.y;
        snapCoords.z = gridCoordinates.z * snapSettings.z;

        transform.position = snapCoords;
    }

    public void ManualSnap(Vector3 gc)
    {
        if (mover == null)
            mover = GetComponent<LerpToPosition>();

        mover.sourcePosition = transform.position;

        if (occupancyCheck)
        {
            //check if you've left your previous grid...
            if (occupiedGridSpot != nullGridCoords)
            {
                if (occupiedGridSpot != gc)
                {
                    SetOccupier(gc, null);
                }
            }

            //if the grid you're going to is already occupied, find the next one accross...
            if (GetOccupier(gc) != null)
            {
                if (GetOccupier(gc) != this)
                {
                    gc.x += 1;
                }

            }
            else
            {
                //gridOccupation[(int)gridCoordinates.x, (int)gridCoordinates.y, (int)gridCoordinates.z] = this;
                SetOccupier(gc, this);
                occupiedGridSpot = gc;
            }
        }


        snapCoords.x = gc.x * snapSettings.x;
        snapCoords.y = gc.y * snapSettings.y;
        snapCoords.z = gc.z * snapSettings.z;


        mover.destination = snapCoords;
        mover.moveSwitch = true;
    }

    [ContextMenu("Snap")]
    public void Snap()
    {
        //Debug.Log("Snapping");

        ManualSnap(gridCoordinates);
    }

    public SmoothSnap GetOccupier(Vector3 gc)
    {
        return gridOccupation[(int)gc.x, (int)gc.y, (int)gc.z];
    }

    public void SetOccupier(Vector3 gc, SmoothSnap s)
    {
        gridOccupation[(int)gc.x, (int)gc.y, (int)gc.z] = s;
    }

    void OnCollisionStay2D(Collision2D col)
    {
       
        SmoothSnap ss = col.gameObject.GetComponent<SmoothSnap>();

        //don't try to occupy the same grid spot
        if(ss != null)
        {
            //Debug.Log("COLLISION BETWEEN SMOOTH SNAPS!!");
            
            if (ss.gridCoordinates == gridCoordinates)
            {
                if (!conflictLeftPush)
                {
                    //prioritise grid spots based on world coordinates.
                    if (ss.transform.position.x < transform.position.x)
                    {
                        //horizontal diference taken into account first..
                        gridCoordinates.x += 1;
                    }
                }
                else
                {
                    //prioritise grid spots based on world coordinates.
                    if (ss.transform.position.x > transform.position.x)
                    {
                        //horizontal diference taken into account first..
                        gridCoordinates.x -= 1;
                    }
                }

                if (ss.transform.position.x == transform.position.x)
                {
                    //if no horizontal difference, look at vertical differnces..

                    if(conflictDownPush)
                    { 
                        if (ss.transform.position.y > transform.position.y)
                            gridCoordinates.y -= 1;
                    }
                    else
                    {
                        if (ss.transform.position.y < transform.position.y)
                            gridCoordinates.y += 1;
                    }
                }

                //yield
                anchorGridCoordinates = gridCoordinates;

                if (isFocus)
                    focusConflictSwitch = true;

                conflictSwitch = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        //SetGridCoordinatesOnPos();
        //Debug.Log("COLLISON EXIT!");

    }

    public void StartDrag()
    {
        Debug.Log("IS FOCUS!!");
        isFocus = true;
    }

    void OnMouseDown()
    {
        StartDrag();
    }

    public void ResetConflictSwitch()
    {
        //disable any custom snapping when you release the mouse over it;
        conflictSwitch = false;
        focusConflictSwitch = false;
        //ReturnToConflictGridCoords();

        Debug.Log("SMOOTHSNAP_ResetConflictSwitch");
    }

    void OnMouseUp()
    {
        ResetConflictSwitch();

        Debug.Log("SmoothSnap:OnMouseUp");
    }
}
