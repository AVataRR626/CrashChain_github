using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LerpToPosition))]
[RequireComponent(typeof(MouseDrag2D))]
[RequireComponent(typeof(SmoothSnap))]
public class CrashLink : MonoBehaviour
{
    public static int overchargeCount = 0;
    public static int crashCount = 0;

    [Header("Basic Attributes")]
    //basic attributes..
    public int coreType;
    public int shellType;

    [Header("Crash Bolt Settings")]
    //crash bolt stuff..
    public CrashBolt myCrashBolt;
    public float boltForce = 400;
    //directions where bolts will be launched
    public bool north = true;
    public bool east = true;
    public bool south = true;
    public bool west = true;

    [Header("Overcharge Behaviours")]
    public GameObject[] deathSpawnList;
    public GameObject[] overchargeSpawnList;    
    public float timeBonus = 0.65f;//extra time player gets before losing
    public float defaultDeathTime = 0.25f;
    public float baseSpinRate = 30;
    public float deathSpinExtension = 0;//extra spin time this chain's overcharge gives
                                    //to other blocks..

    [Header("System Settings")]
    //system things..
    public CrashLinkEditor myEditor;
    public float doubleClickTime = 0.5f;
    public float touchFactor = 1;
    public float charge;
    public float chargeLimit = 2;    
    public Transform graphicsRoot;

    private LerpToPosition mover;
    private MouseDrag2D dragger;
    private Vector3 mouseDownGrid;
    private bool chargeSwitch = false;
    private float rotationRate;
    private float killClock = -1;
    private SmoothSnap smoothSnap;
    private float lastClickTime;
    private Quaternion initialGraphicsRotation;
    private TypeMaster myTypeMaster;

    void Awake()
    {
        myTypeMaster = FindObjectOfType<TypeMaster>();

        ColourOutlines(transform);
    }

    // Use this for initialization
    void Start()
    {
        if (myEditor == null)
            myEditor = FindObjectOfType<CrashLinkEditor>();

        overchargeCount = 0;

        dragger = GetComponent<MouseDrag2D>();

        if(graphicsRoot == null)
        {
            graphicsRoot = transform.Find("Graphics");
        }

        initialGraphicsRotation = graphicsRoot.rotation;

        smoothSnap = GetComponent<SmoothSnap>();

        myTypeMaster = FindObjectOfType<TypeMaster>();

        
        //Don't allow non-square CrashLinks to have blocked directions.
        if(myCrashBolt.type != 0)
        {
            north = true;
            east = true;
            west = true;
            south = true;
        }
    }

    public void ColourOutlines(Transform t)
    {
        if(t.name == "Outline")
        {
            t.GetComponent<SpriteRenderer>().color = myTypeMaster.outlineColour;
        }

        foreach(Transform child in t)
        {
            ColourOutlines(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(killClock <= 0)
            ManageCharge();

        ManageSpin();
        ManageKillTriggers();
    }

    public void Kill()
    {
        Kill(defaultDeathTime);
    }

    //kills the crash chain..
    //t is the countdown timer;
    public void Kill(float t)
    {
        SpawnOverchargeList();
        chargeSwitch = true;
        killClock = t;
        charge = chargeLimit;

        if(deathSpinExtension > 0)
            ExtendOtherDeathTimes();
    }

    public void ExtendOtherDeathTimes()
    {
        CrashLink[] links = FindObjectsOfType<CrashLink>();

        foreach(CrashLink l in links)
        {
            if(l != this)
            {
                l.DeathSpinExtend(deathSpinExtension);
            }
        }
    }

    public void DeathSpinExtend(float t)
    {
        if(killClock > 0)
            killClock += t;
    }
        
    public void SpawnCrashBolt(Vector2 force)
    {
        CrashBolt c;
        c = Instantiate(myCrashBolt, transform.position, Quaternion.identity) as CrashBolt;
        c.type = coreType;
        c.GetComponent<Rigidbody2D>().AddForce(force);
    }

    void ManageKillTriggers()
    {
        if(killClock != -1)
        {
            if (killClock >= 0)
            {
                killClock -= Time.deltaTime;
            }
            else
            {
                //launch the crash bolts
                Vector2 force;
                if (north)
                {   
                    force = new Vector2(0, boltForce);                    
                    SpawnCrashBolt(force);
                }

                if (south)
                {
                    force = new Vector2(0, -boltForce);
                    SpawnCrashBolt(force);
                }

                if (east)
                {
                    force = new Vector2(boltForce,0);
                    SpawnCrashBolt(force);
                }

                if (west)
                {
                    force = new Vector2(-boltForce, 0);
                    SpawnCrashBolt(force);
                }


                SpawnDeathList();

                // Debug.Log("Adding " + timeBonus + " to " + OverchargeMonitor.timeLimit + " ");
                OverchargeMonitor.instance.AddToClock(timeBonus);
                Destroy(gameObject);
                crashCount++;
            }
        }
        else
        {
            if (charge >= chargeLimit)
            {
                overchargeCount++;
                Kill();
            }
        }
    }

    void SpawnDeathList()
    {
        //generate the death spawn list
        if (deathSpawnList != null)
        {
            for (int i = 0; i < deathSpawnList.Length; i++)
            {
                Instantiate(deathSpawnList[i], transform.position, Quaternion.identity);
            }
        }
    }

    void SpawnOverchargeList()
    {
        for (int i = 0; i < overchargeSpawnList.Length; i++)
        {
            Instantiate(overchargeSpawnList[i], transform.position, transform.rotation);
        }
    }

    void ManageSpin()
    {
        if (chargeSwitch || charge >= chargeLimit)
        {
            rotationRate = baseSpinRate * charge * charge;

            if(graphicsRoot != null)
                graphicsRoot.transform.Rotate(0, 0, rotationRate * Time.deltaTime);
        }
        else
        {
            if (graphicsRoot != null)
                graphicsRoot.transform.rotation = initialGraphicsRotation;
        }


    }

    void ManageCharge()
    {
        if (chargeSwitch)
        {
            charge += (Time.deltaTime * touchFactor) + Time.deltaTime;
        }
        else
        {
            charge = 0;
        }

        if (charge < chargeLimit)
        {

            if (charge > 0)
            {
                charge -= Time.deltaTime;
            }
            else
            {
                charge = 0;
            }
        }
    }

    public void StartDrag()
    {

        //GetComponent<MouseDrag2D>().StartDrag();

       
        mouseDownGrid = new Vector3(0,0,0);

        if(smoothSnap == null)
            smoothSnap = GetComponent<SmoothSnap>();

        mouseDownGrid = smoothSnap.gridCoordinates;


        //double click/tap activates the spinning!

        if (Time.time - lastClickTime <= doubleClickTime)
        {
            Debug.Log("CrashLink: click time diff: " + (Time.time - lastClickTime));
            charge = chargeLimit;
            chargeSwitch = true;
        }

        lastClickTime = Time.time;

        //stuff in editor mode
        if (myEditor != null)
        {
            //notify that you are the focus
            myEditor.myFocus = this;

            //defocus and deglow everything else..
            CrashLink[] allTheLinks = FindObjectsOfType<CrashLink>();

            foreach (CrashLink c in allTheLinks)
            {
                c.BroadcastMessage("TouchDim");
            }

        }

        //glow yourself...
        gameObject.BroadcastMessage("TouchGlow");

    }

    void OnMouseDown()
    {
        StartDrag();
        
    }


    void OnMouseUp()
    {   
        chargeSwitch = false;

        //don't automatically dim if in edit mode...
        if(myEditor == null)
            gameObject.BroadcastMessage("TouchDim");

    }

    //returns the CrashLink format serialisation.
    //please see CrashLinkEditor for exact formatting reference...
    public string Serialise()
    {
        string result = "";

        //Default crash links are square.
        string linkMode = "Q";

        //Assume Hex Crash bolts have shell converters
        if (myCrashBolt.opMode == CrashBolt.OperationMode.ConvertShell)
            linkMode = "H";

        //Assume Triangle Crash bolts have core covnerters
        if (myCrashBolt.opMode == CrashBolt.OperationMode.ConvertCore)
            linkMode =  "T";

        //Build link type, shell and core type and grid coordinates...
        result += linkMode + "|";
        result += shellType.ToString() + "|";
        result += coreType.ToString() + "|";
        result += smoothSnap.gridCoordinates.x.ToString() + "|";
        result += smoothSnap.gridCoordinates.y.ToString() + "|";

        //Now take a look at all the directions
        bool allFree = true;

        //Don't allow non-square CrashLinks to have blocked directions.
        if (myCrashBolt.type != 0)
        {
            north = true;
            east = true;
            west = true;
            south = true;
        }

        if (!north)
        {
            allFree = false;
            result += "N";
        }

        if (!east)
        {
            allFree = false;
            result += "E";
        }

        if (!west)
        {
            allFree = false;
            result += "W";
        }

        if (!south)
        {
            allFree = false;
            result += "S";
        }

        if (allFree)
            result += "F";

        //DOONE!, now go ahead and return the result..

        return result;
    }

    //applies serialised settings based on CrashLink formatting.    
    //please see CrashLinkEditor for exact formatting reference...
    public void Deserialise(string serialisation)
    {
        //Debug.Log("Deserialsing: " + serialisation);
        string [] attributes;
        char [] delim = { '|' };

        attributes = serialisation.Split(delim);

        //apply the shell and core types
        shellType = int.Parse(attributes[1]);
        coreType = int.Parse(attributes[2]);

        //apply coordinates
        if (smoothSnap == null)
            smoothSnap = GetComponent<SmoothSnap>();

                
        smoothSnap.gridCoordinates.x = int.Parse(attributes[3]);
        smoothSnap.gridCoordinates.y = int.Parse(attributes[4]);
        smoothSnap.InstantSnap();

        //and now apply direction bocks..
        if (attributes[5].Contains("N"))
            north = false;

        if (attributes[5].Contains("E"))
            east = false;

        if (attributes[5].Contains("W"))
            west = false;

        if (attributes[5].Contains("S"))
            south = false;
    }

}

