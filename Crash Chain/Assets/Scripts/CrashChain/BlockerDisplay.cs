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
    public bool arrowMode = false;
    public Color blinkColour;

    private SpriteRenderer mySpr;
    Color origColour;

    void Awake()
    {
        GetComponents();
        CheckDirection();

        origColour = mySpr.color;
    }

    void GetComponents()
    {
        if (myLink == null)
            myLink = transform.root.GetComponent<CrashLink>();

        if (mySpr == null)
            mySpr = GetComponent<SpriteRenderer>();

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
        GetComponents();

        CheckDirection();
    }


    public void BlinkOn()
    {
        mySpr.color = blinkColour;
    }

    public void BlinkOff()
    {
        mySpr.color = origColour;
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
                    mySpr.enabled = !myLink.north ^ arrowMode;

                if (lcname.Contains("east"))
                    mySpr.enabled = !myLink.east ^ arrowMode;

                if (lcname.Contains("south"))
                    mySpr.enabled = !myLink.south ^ arrowMode;

                if (lcname.Contains("west"))
                    mySpr.enabled = !myLink.west ^ arrowMode;

                if (myLink.movable)
                {
                    if (lcname.Contains("horizontal"))
                        mySpr.enabled = !myLink.horizontalDrag;

                    if (lcname.Contains("vertical"))
                        mySpr.enabled = !myLink.verticalDrag;

                }
                else
                {
                    //don't interfere with the outline graphics
                    if (lcname.Contains("horizontal") || lcname.Contains("vertical"))
                        mySpr.enabled = false;
                }
                
            }
        }
    }
}
