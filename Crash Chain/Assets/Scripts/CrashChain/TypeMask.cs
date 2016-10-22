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
    public float syncColourDelay = 0;

    public bool touchSwitch = false;

    private Color touchColourAdd = new Color(0.2f, 0.2f, 0.2f);

    private ImageFadeIn fader;

    void Awake()
    {
        if (myCrashLink == null)
            myCrashLink = transform.root.GetComponent<CrashLink>();

        if (myCrashBolt == null)
            myCrashBolt = transform.root.GetComponent<CrashBolt>();

        spr = GetComponent<SpriteRenderer>();
        fader = GetComponent<ImageFadeIn>();
        ForceSyncColours();
    }

    // Use this for initialization
    void Start ()
    {
        if (myCrashLink == null)
            myCrashLink = transform.root.GetComponent<CrashLink>();

        if (myCrashBolt == null)
            myCrashBolt = transform.root.GetComponent<CrashBolt>();

        spr = GetComponent<SpriteRenderer>();
        myTypeMaster = TypeMaster.Instance;

        //ForceSyncColours();
        Invoke("FaderPrep", 0.1f);
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

         SyncColours();

    }

    public void FaderPrep()
    {
        if(fader != null)
        {
            ForceSyncColours();
            Color col = spr.color;
            col.a = 0;
            spr.color = col;
        }
    }

    public void ForceSyncColours()
    {
        myTypeMaster = FindObjectOfType<TypeMaster>();

        if (myTypeMaster != null)
        {

            //Debug.Log("Found My Master");

            Color col = myTypeMaster.typeColours[type];

            if (touchSwitch)
                col += touchColourAdd;

            spr.color = col;
        }
    }

    void SyncColours()
    {
        if (myTypeMaster != null)
        {
            Color col = myTypeMaster.typeColours[type];

            if (touchSwitch)
                col += touchColourAdd;

            if(Application.isPlaying)
            {
                if(syncColourDelay > 0)
                {
                    syncColourDelay -= Time.deltaTime;
                    return;
                }
            }

            spr.color = col;
        }
        else
        {
            myTypeMaster = FindObjectOfType<TypeMaster>();
        }
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


    public void TypeCycle()
    {
        type++;

        if (type > 2)
            type = 0;
    }
}
