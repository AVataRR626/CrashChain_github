using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class TypeMask : MonoBehaviour
{
    public bool coreMode = false;

    
    public int type = 0;
    public CrashLink myCrashLink;
    public CrashBolt myCrashBolt;
    public SpriteRenderer spr;
    public TypeMaster myTypeMaster;

    public bool touchSwitch = false;

    private Color touchColourAdd = new Color(0.2f, 0.2f, 0.2f);

    void Awake()
    {
        if (myCrashLink == null)
            myCrashLink = transform.root.GetComponent<CrashLink>();

        if (myCrashBolt == null)
            myCrashBolt = transform.root.GetComponent<CrashBolt>();
    }

    // Use this for initialization
    void Start ()
    {


	    if(myCrashLink == null)
            myCrashLink = transform.root.GetComponent<CrashLink>();

        if (myCrashBolt == null)
            myCrashBolt = transform.root.GetComponent<CrashBolt>();

        spr = GetComponent<SpriteRenderer>();
        myTypeMaster = TypeMaster.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(myTypeMaster == null)
            myTypeMaster = TypeMaster.Instance;

        if (myCrashLink != null)
        { 
            if (coreMode)
                type = myCrashLink.coreType;
            else
                type = myCrashLink.shellType;
        }
        else if (myCrashBolt != null)
        {
            type = myCrashBolt.type;
        }

        Color col = myTypeMaster.typeColours[type];

        if (touchSwitch)
            col += touchColourAdd;

        spr.color = col;
	}

    void TouchGlow()
    {
        //Debug.Log("TypeMask OnMouseDown");
        touchSwitch = true;
    }

    void TouchDim()
    {
        touchSwitch = false;
    }
}
