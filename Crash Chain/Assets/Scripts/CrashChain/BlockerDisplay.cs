using UnityEngine;
using System.Collections;


//purely a graphical indicator thingy..
//activates/deactivates the graphics based on
//which directions are active for a CrashLink.
//
//activations based on name.
//
public class BlockerDisplay : MonoBehaviour
{

    public CrashLink myLink;

    private SpriteRenderer mySpr;

    void Awake()
    {
        if (myLink == null)
            myLink = transform.root.GetComponent<CrashLink>();

        if(mySpr == null)
            mySpr = GetComponent<SpriteRenderer>();

        CheckDirection();
    }

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckDirection();
    }

    void OnDrawGizmos()
    {
        if (myLink == null)
            myLink = transform.root.GetComponent<CrashLink>();

        if (mySpr == null)
            mySpr = GetComponent<SpriteRenderer>();

        CheckDirection();
    }


    public void CheckDirection()
    {
        if (mySpr != null)
        {
            mySpr.enabled = false;

            if (myLink != null)
            {
                string lcname = name.ToLower();


                if (lcname.Contains("north"))
                    mySpr.enabled = !myLink.north;

                if (lcname.Contains("east"))
                    mySpr.enabled = !myLink.east;

                if (lcname.Contains("south"))
                    mySpr.enabled = !myLink.south;

                if (lcname.Contains("west"))
                    mySpr.enabled = !myLink.west;

            }
        }
    }
}
