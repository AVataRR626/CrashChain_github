//Matt Cabanag
//Feb 2016
//General movement utility

using UnityEngine;
using System.Collections;

public class LerpToPosition : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 sourcePosition;
    public float lerpTime = 3;


    private float dist2dest = 0;

    public bool moveSwitch = false;
    public float lerpClock = 0;
    public float lerpValue = 0;

    // Use this for initialization
    void Start()
    {
        destination = transform.position;
        sourcePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        dist2dest = Vector3.Distance(destination, transform.position);

        if(moveSwitch)
            MoveToDestination();
    }

    void MoveToDestination()
    {
        if(destination != sourcePosition)
        { 
            //move to your destination
            lerpClock += Time.deltaTime;


            lerpValue = lerpClock / lerpTime;

            if (lerpValue > 1)
            {
                lerpValue = 1;
                transform.position = destination;
            }

            Vector3 newPos = Vector3.Lerp(sourcePosition, destination, lerpValue);
            transform.Translate(newPos - transform.position, Space.Self);

            //you have arrived!!
            if ((destination - transform.position).magnitude <= 0.1f)
            {
                transform.position = destination;//get rid of any rounding errors in the comparison.
                sourcePosition = destination;
                moveSwitch = false;
                lerpClock = 0;
                lerpValue = 0;
            }
        }

    }

    [ContextMenu("StartMove")]
    void StartMove()
    {
        sourcePosition = transform.position;
        dist2dest = Vector3.Distance(destination, sourcePosition);
        lerpClock = 0;

    }



    /*
    acceleration based movement stuff:

    public float arrivalRange = 0;
    public float maxSpeed = 3;
    public float acceleration = 0.5f;
    public float moveSpeed;
    public bool moving = false;

    void MoveToDestination()
    {


        if (dist2dest > arrivalRange)
        {
            //transform.position = Vector3.Lerp (transform.position,destination.position,flightSpeed*Time.deltaTime);


            Vector3 trans = destination - transform.position;
            trans.Normalize();
            transform.Translate(trans*moveSpeed);

        }

    }

    
    void HandleAcceleration()
    {
        if (dist2dest > moveSpeed)
        {
            if (moveSpeed < maxSpeed)
                moveSpeed += acceleration * Time.deltaTime;
            else
                moveSpeed = maxSpeed;
        }
        else
        {
            if (moveSpeed > 0)
                moveSpeed -= acceleration * Time.deltaTime;
            else if (moveSpeed < 0)
                moveSpeed = 0;
        }
    }*/

}
