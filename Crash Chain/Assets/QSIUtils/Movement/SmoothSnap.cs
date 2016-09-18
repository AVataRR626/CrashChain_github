﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LerpToPosition))]
public class SmoothSnap : MonoBehaviour
{
    public static SmoothSnap [,,] gridOccupation = new SmoothSnap[200,200,200];
    public static SmoothSnap[,,] desiredGridOccupation = new SmoothSnap[200, 200, 200];
    public static Vector3 nullGridCoords = new Vector3(-1, -1, -1);
    public static bool focusConflictSwitch = false;

    LerpToPosition mover;

    public Vector3 snapSettings = new Vector3(1, 1, 1);
    public bool noSnapOverride = false;
    public bool verticalLock = false;
    public bool horizontalLock = false;
    public Vector3 lockAnchor;

    //should be private, but kept public for debugging...
    public bool snapSwitch = true;
    public bool occupancyCheck = false;
    public bool conflictSwitch = false;
    public float borderHeight = 0.75f;
    public float borderWidth = 0.75f;

    public Vector3 gridCoordinates;
    public Vector3 anchorGridCoordinates;

    Vector3 snapCoords;
    Vector3 occupiedGridSpot = new Vector3(-1,-1,-1);
    bool isFocus = false;

    Vector3 originalAnchor;

    MouseDrag2D dragger;

    public bool VerticalLock
    {
        get
        {
            return verticalLock;
        }

        set
        {
            
            lockAnchor = gridCoordinates;
            verticalLock = value;
        }
    }

    public bool HorizontalLock
    {
        get
        {
            return horizontalLock;
        }

        set
        {   
            lockAnchor = gridCoordinates;
            horizontalLock = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        mover = GetComponent<LerpToPosition>();
        dragger = GetComponent<MouseDrag2D>();

        SetGridCoordinatesOnPos();
        anchorGridCoordinates = gridCoordinates;
        originalAnchor = anchorGridCoordinates;
        lockAnchor = originalAnchor;
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
            if(isFocus)
            {
                anchorGridCoordinates = gridCoordinates;
                isFocus = false;
            }
        }

        //horizontal lock means the horizontal axis can't be moved
        if (horizontalLock)
        {
            anchorGridCoordinates.x = lockAnchor.x;

            if(gridCoordinates.x != anchorGridCoordinates.x)
                snapSwitch = true;
        }

        //vertical lock means the vertical axis can't be moved
        if (verticalLock)
        {
            anchorGridCoordinates.y = lockAnchor.y;

            if (gridCoordinates.y != anchorGridCoordinates.y)
                snapSwitch = true;
        }


        if (transform.position != snapCoords && snapSwitch)
            Snap();

        mover.moveSwitch = snapSwitch;

        SetGridCoordinatesOnPos();

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

        ManualSnap(anchorGridCoordinates);
    }

    public SmoothSnap GetOccupier(Vector3 gc)
    {
        return gridOccupation[(int)gc.x, (int)gc.y, (int)gc.z];
    }

    public void SetOccupier(Vector3 gc, SmoothSnap s)
    {
        gridOccupation[(int)gc.x, (int)gc.y, (int)gc.z] = s;
    }

    public float DistanceToXGridCoordinates()
    {
        return Mathf.Abs(transform.position.x - snapCoords.x);
    }

    public float DistanceToYGridCoordinates()
    {
        return Mathf.Abs(transform.position.y - snapCoords.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {   
        OnCollisionStay2D(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        SmoothSnap ss = col.gameObject.GetComponent<SmoothSnap>();

        //HandleConflictA(ss);
        if (ss != null)
        {
            if (!ss.horizontalLock && !horizontalLock && !ss.verticalLock && !ss.horizontalLock)
            {
                HandleConflictB(ss);
            }
            else
            {
                HandleAxisLockedConflict(ss);
            }
        }
    }

    public void HandleAxisLockedConflict(SmoothSnap ss)
    {

        if ((horizontalLock == ss.horizontalLock && verticalLock == ss.verticalLock))
        {
            //if you've got the same axis locks, act as if you have no locks...
            if (ss.gridCoordinates == gridCoordinates)
            {
                HandleConflictB(ss);
            }
        }
        else
        {
            HandleConflictB(ss);

            if (dragger != null)
            {
                if(ss.horizontalLock)
                {
                    if (Mathf.Abs(transform.position.x - ss.transform.position.x) > borderWidth * 2)
                    {
                        dragger.horizontalBlock = true;
                    }
                }


                if(ss.verticalLock)
                {
                    if(Mathf.Abs(transform.position.y - ss.transform.position.y) > borderHeight*2)
                    {   
                        dragger.verticalBlock = true;
                    }
                }
            }

            conflictSwitch = true;
        } 
    }

    public void HandleConflictB(SmoothSnap ss)
    {
        if (ss != null)
        {
            if (ss.anchorGridCoordinates == anchorGridCoordinates)
            {
                //closest to grid coordinates is the priority
                if(ss.DistanceToXGridCoordinates() < DistanceToXGridCoordinates())
                {
                    if(transform.position.x < ss.transform.position.x)
                    {
                        anchorGridCoordinates.x -= 1;
                    }
                    else
                    {
                        anchorGridCoordinates.x += 1;
                    }
                }
                else if(ss.DistanceToYGridCoordinates() < DistanceToYGridCoordinates())
                {
                    if (transform.position.y < ss.transform.position.y)
                    {
                        anchorGridCoordinates.y -= 1;
                    }
                    else
                    {
                        anchorGridCoordinates.y += 1;
                    }
                }

                //yield                
                conflictSwitch = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        //SetGridCoordinatesOnPos();
        //Debug.Log("COLLISON EXIT!");

        
        if(!verticalLock)
            dragger.verticalBlock = false;

        if (!horizontalLock)
            dragger.horizontalBlock = false;
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
        SetAnchorGridCoordinatesOnPos();
        //ReturnToConflictGridCoords();

        Debug.Log("SMOOTHSNAP_ResetConflictSwitch");
    }

    void OnMouseUp()
    {
        ResetConflictSwitch();

        Debug.Log("SmoothSnap:OnMouseUp");
    }
}
