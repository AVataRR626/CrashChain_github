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
    public bool tappable = true;
    public bool verticalDrag = true;
    public bool horizontalDrag = true;
    public bool movable = true;

    public bool HorizontalDrag
    {
        get
        {
            return horizontalDrag;
        }

        set
        {
            horizontalDrag = value;
            SyncAxisLocks();
        }
    }

    public bool VerticalDrag
    {
        get
        {
            return verticalDrag;
        }

        set
        {
            verticalDrag = value;
            SyncAxisLocks();
        }
    }

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
    public bool screenShake = true;
    public GameObject[] deathSpawnList;
    public GameObject[] overchargeSpawnList;
    public float timeBonus = 0.65f;//extra time player gets before losing
    public float defaultDeathTime = 0.25f;
    public float baseSpinRate = 30;
    public float spinAcceleration = 900;
    public float deathSpinExtension = 0;//extra spin time this chain's overcharge gives
                                        //to other blocks..

    [Header("System Settings")]
    //system things..
    public CrashLinkEditor myEditor;
    public float tapTime = 0.1f;
    public float tapDistance = 0.1f;
    public float touchFactor = 1;
    public float charge;
    public float chargeLimit = 2;
    public Transform graphicsRoot;
    public float colliderActivateDelay = 0.25f;
    public int [] bitSerialisation;
    public string bitStringEncoding;
    public ScalePulse touchPulser;

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

    private Vector3 prevAnchorGridCoordinates;
    public float holdCharge = 0;
    public Vector3 prevPos;
    public float dragDistance;
    
    private bool draggedSwitch = false;

    void Awake()
    {
        myTypeMaster = FindObjectOfType<TypeMaster>();
        ColourOutlines();

        if(!movable)
        {
            horizontalDrag = false;
            verticalDrag = false;
        }

        touchPulser = GetComponent<ScalePulse>();
    }

    // Use this for initialization
    void Start()
    {

        if (myEditor == null)
            myEditor = FindObjectOfType<CrashLinkEditor>();

        //reset overcharge if you're not in arcade mode..
        //bit hacky way of doing things, but don't care rn.
        if (FindObjectOfType<CrashChainArcadeManager>() == null)
            overchargeCount = 0;

        dragger = GetComponent<MouseDrag2D>();
        mover = GetComponent<LerpToPosition>();

        if (graphicsRoot == null)
        {
            graphicsRoot = transform.Find("Graphics");
        }

        initialGraphicsRotation = graphicsRoot.rotation;

        smoothSnap = GetComponent<SmoothSnap>();
        prevAnchorGridCoordinates = smoothSnap.gridCoordinates;

        myTypeMaster = FindObjectOfType<TypeMaster>();


        //Don't allow non-square CrashLinks to have blocked directions.
        //Or restricted axes
        if (myCrashBolt.type != 0)
        {
            north = true;
            east = true;
            west = true;
            south = true;
        }

        ColourOutlines(transform);

        if (colliderActivateDelay > 0)
            GetComponent<Collider2D>().enabled = false;

        if (!movable)
        {
            horizontalDrag = false;
            verticalDrag = false;
        }

        Invoke("SetDraggerStarPos", 0.5f);
        SyncAxisLocks();
    }

    public void SyncAxisLocks()
    {
        //Debug.Log("AXIS LOCK SYNC!");

        if (dragger == null)
            dragger = GetComponent<MouseDrag2D>();

        if (smoothSnap == null)
            smoothSnap = GetComponent<SmoothSnap>();

        dragger.horizontalBlock = !horizontalDrag;
        dragger.verticalBlock = !verticalDrag;
        smoothSnap.VerticalLock = !verticalDrag;
        smoothSnap.HorizontalLock = !horizontalDrag;
    }

    public void SetDraggerStarPos()
    {
        dragger.startingPos = transform.position;
    }

    public void ColourOutlines()
    {
        ColourOutlines(transform);
    }

    public void ColourOutlines(Transform t)
    {
        if (t.name == "Outline")
        {
            if (t.parent.name == "Shell")
            {
                if (movable)
                    t.GetComponent<SpriteRenderer>().color = myTypeMaster.outlineColour;
                else
                    t.GetComponent<SpriteRenderer>().color = myTypeMaster.immovableColour;
            }

            if (t.parent.name == "Core")
            {
                if (tappable)
                    t.GetComponent<SpriteRenderer>().color = myTypeMaster.outlineColour;
                else
                    t.GetComponent<SpriteRenderer>().color = myTypeMaster.immovableColour;
            }
        }

        foreach (Transform child in t)
        {
            ColourOutlines(child);
        }
    }

    void OnDrawGizmos()
    {

        if (myTypeMaster == null)
            myTypeMaster = FindObjectOfType<TypeMaster>();

        ColourOutlines(transform);

        if (!verticalDrag && !horizontalDrag)
            movable = false;
        else
            movable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (killClock <= 0)
            ManageCharge();


        dragger.enabled = movable;
        smoothSnap.GetComponent<Rigidbody2D>().isKinematic = !movable;
        ManageSpin();
        ManageKillTriggers();
        MonitorMoves();

        if (!Input.GetMouseButton(0))
            if (holdCharge > 0)
                holdCharge -= Time.deltaTime;

        if (Input.GetMouseButtonUp(0))
            smoothSnap.SetAnchorGridCoordinatesOnPos();


        if (colliderActivateDelay > 0)
            colliderActivateDelay -= Time.deltaTime;
        else if (!GetComponent<Collider2D>().enabled)
            GetComponent<Collider2D>().enabled = true;


        if (!verticalDrag && !horizontalDrag)
            movable = false;
        else
            movable = true;

        ColourOutlines();

        if (OverchargeMonitor.instance.RemainingOvercharges() == 0)
        {
            dragger.enabled = false;
        }
    }

    void MonitorMoves()
    {
        if(OverchargeMonitor.crashBoltCount <= 0)
        { 
            if (prevAnchorGridCoordinates != smoothSnap.anchorGridCoordinates)
            {
                OverchargeMonitor.instance.AddMove();
                draggedSwitch = false;

                if (myEditor != null)
                    myEditor.SetUnsavedIndicator(true);
            }
        }

        prevAnchorGridCoordinates = smoothSnap.anchorGridCoordinates;
    }

    public void Kill()
    {
        //Pulse();

        if (screenShake)
        {

            PerlinShake shaker = Camera.main.GetComponent<PerlinShake>();
            
            if(shaker != null)
                shaker.StartShake();
        }

        Kill(defaultDeathTime);
    }

    //kills the crash chain..
    //t is the countdown timer;
    public void Kill(float t)
    {
        if (touchPulser != null)
        {
            touchPulser.pulseTime *= 1.1f;
            touchPulser.scaleFactor *= 1.1f;
        }

        PulseUp();

        SpawnOverchargeList();
        chargeSwitch = true;
        killClock = t;
        charge = chargeLimit;

        if (deathSpinExtension > 0)
            ExtendOtherDeathTimes();
    }

    public void ExtendOtherDeathTimes()
    {
        CrashLink[] links = FindObjectsOfType<CrashLink>();

        foreach (CrashLink l in links)
        {
            if (l != this)
            {
                l.DeathSpinExtend(deathSpinExtension);
            }
        }
    }

    public void DeathSpinExtend(float t)
    {
        if (killClock > 0)
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
        if (killClock != -1)
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
                    force = new Vector2(boltForce, 0);
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

    public void Pulse()
    {
        if (touchPulser != null)
            touchPulser.Pulse();
    }

    public void PulseUp()
    {
        if (touchPulser != null)
            touchPulser.PulseUp();
    }

    public void PulseDown()
    {
        if (touchPulser != null)
            touchPulser.PulseDown();
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
            
            rotationRate = baseSpinRate * charge;
            baseSpinRate += spinAcceleration * Time.deltaTime;

            if (graphicsRoot != null)
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
        if (myEditor == null)
            myEditor = FindObjectOfType<CrashLinkEditor>();

        if(myEditor != null)
        {
            //don't engage overcharge when you're in edit mode.
            if (myEditor.testMode)
            {
                chargeSwitch = false;
                return;
            }
        }

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
        SetDraggerStarPos();

        mouseDownGrid = new Vector3(0, 0, 0);

        if (smoothSnap == null)
            smoothSnap = GetComponent<SmoothSnap>();

        mouseDownGrid = smoothSnap.gridCoordinates;


        /*
         * //double click/tap activates the spinning!
        if (Time.time - lastClickTime <= doubleClickTime)
        {
            //Debug.Log("CrashLink: click time diff: " + (Time.time - lastClickTime));
            charge = chargeLimit;
            chargeSwitch = true;
        }*/

        lastClickTime = Time.time;
    }

    public static bool CoinFlip()
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f)
            return true;

        return false;
    }

    public void RandomiseBasicStatsEZ1()
    {
        RandomiseBlockers();
        RandomiseCore(2);
        RandomiseShell(2);
    }

    public void RandomiseBasicStatsEZ2()
    {
        RandomiseBlockers();
        RandomiseCore(3);
        RandomiseShell(2);
        RandomiseTapability();
    }


    public void RandomiseBasicStats()
    {
        RandomiseBlockers();
        RandomiseCore();
        RandomiseShell();
        RandomiseTapability();
    }

    public void RandomiseStats()
    {   
        RandomiseBasicStats();
        RandomiseMovability();
        
    }

    public void RandomiseMovability()
    {
        horizontalDrag = CoinFlip();
        verticalDrag = CoinFlip();
        SyncAxisLocks();
    }

    public void RandomiseTapability()
    {
        tappable = CoinFlip();
    }

    public void RandomiseBlockers()
    {
        north = CoinFlip();
        east = CoinFlip();
        west = CoinFlip();
        south = CoinFlip();
    }

    public void RandomiseCore()
    {
        RandomiseCore(3);
    }

    public void RandomiseShell()
    {
        RandomiseShell(3);
    }

    public void RandomiseCore(int max)
    {
        coreType = Random.Range(0, max);
    }

    public void RandomiseShell(int max)
    {
        shellType = Random.Range(0, max);
    }

    void OnMouseDown()
    {
        ArrowPulser.pulseMode = false;

        if (myEditor == null)
            myEditor = FindObjectOfType<CrashLinkEditor>();

        if(myEditor != null)
        {

            Debug.Log("CrashLink: myEditor is not null");
            if (!myEditor.testMode)
            {
                holdCharge = tapTime * 3;
                Debug.Log("CrashLink: I am the focus:");
                myEditor.myFocus = this;

                myEditor.blockFrameClickCount += 1;
            }
        }

        PulseUp();

        lastClickTime = Time.time;

        StartDrag();

        //glow yourself...
        gameObject.BroadcastMessage("TouchGlow");

        dragDistance = 0;
        prevPos = transform.position;
    }

    void OnMouseUp()
    {
        ArrowPulser.pulseMode = true;

        PulseDown();

        //immovable blocks can't be tapped
        if (holdCharge <= tapTime && 
            dragDistance <= tapDistance &&
            tappable)
        {
            if (Time.time - lastClickTime <= tapTime)
            {
                charge = chargeLimit;
                chargeSwitch = true;
            }
            else
            {
                chargeSwitch = false;
                lastClickTime = Time.time;
            }
        }


        gameObject.BroadcastMessage("TouchDim");
        SyncAxisLocks();
        dragDistance = 0;
        prevPos = transform.position;

        draggedSwitch = true;
    }

    void OnMouseDrag()
    {
        if (holdCharge < 0.25f)
        {
            holdCharge += Time.deltaTime;
            //Debug.Log("--------------------- hold charge");
        }

        dragDistance += Vector3.Distance(transform.position,prevPos);
        prevPos = transform.position;
    }

    //returns the CrashLink format serialisation.
    //please see CrashLinkEditor for exact formatting reference...
    public string Serialise()
    {
        string result = "";

        //Default crash links are square.
        string linkMode = "Q";

        //Assume Hex Crash bolts have core converters
        if (myCrashBolt.opMode == CrashBolt.OperationMode.TunnelCore)
            linkMode = "H";

        //Assume Triangle Crash bolts have shell covnerters
        if (myCrashBolt.opMode == CrashBolt.OperationMode.ConvertShell)
            linkMode = "T";


        string movability = "M";//M is for movable;
        if (!movable)
            movability = "I";//I is for immovable
        else
        {
            if(horizontalDrag && verticalDrag)
            {
                movability = "M";
            }
            else if(horizontalDrag)
            {
                movability = "Z";
            }
            else if(verticalDrag)
            {
                movability = "V";
            }

        }

        string tapability = "B";//B is for tappable
        if (!tappable)
            tapability = "U";//U is for untappable

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

        result += "|";
        //DOONE!, now go ahead and return the result..

        result += movability + "|";
        result += tapability + "|";

        return result;
    }

    [ContextMenu("BitSerialise")]
    public string BitSerialise()
    {
        byte primaryInfo = 0;//type,shell,core,hrestrict,vrestrict
        byte launchStats = 0;//tappable, north, east, west, south
        byte xGrid = 0;//xgrid coordinate
        byte yGrid = 0;//ygrid coordinate

        //convert the first part...
        byte type = (byte)LinkType(); //0000 00xx
        byte core = (byte)(coreType << 2); //0000 xx00
        byte shell = (byte)(shellType << 4); //00xx 0000
        byte h = (byte)((horizontalDrag ? 1 : 0) << 6);//0x00 0000
        byte v = (byte)((verticalDrag ? 1 : 0) << 7);//x000 0000
        primaryInfo = (byte)(type | core | shell | h | v);

        //now the second part!!
        byte t = (byte)(tappable ? 1 : 0);//0000 000x
        byte n = (byte)((north ? 1 : 0) << 1);//0000 00x0
        byte e = (byte)((east ? 1 : 0) << 2);//0000 0x00
        byte w = (byte)((west ? 1 : 0) << 3);//0000 x000
        byte s = (byte)((south ? 1 : 0) << 4);//000x 0000
        launchStats = (byte)(t | n | e | w | s);

        //now store the grid coordinates
        xGrid = (byte)smoothSnap.gridCoordinates.x;
        yGrid = (byte)smoothSnap.gridCoordinates.y;

        
        //truncate coordinate data into the remaining 3 bits of launchStats
        byte xP1 = (byte)(xGrid << 5);//xxx0 0000 (the last part of launchStats)
        launchStats = (byte)(launchStats | xP1);

        //store the rest along with the yGrid parts..
        byte xP2 = (byte)(xGrid >> 3);//0000 0xxx     0000 0111 
        byte yGridShifted = (byte) (yGrid << 3);//xxxx x000

        byte yGridXPart = (byte)(yGridShifted | xP2);
        

        char[] resultArr = new char[3];
        resultArr[0] = (char)primaryInfo;
        resultArr[1] = (char)launchStats;
        resultArr[2] = (char)yGridXPart;
       
        //resultArr[2] = (char)xGrid;
        //resultArr[3] = (char)yGrid;

        

        Debug.Log("----------- " + primaryInfo + ", " + launchStats + ", " + (int)resultArr[2] + ", " + xGrid + ", " + yGrid);

        bitSerialisation = new int[resultArr.Length];

        for(int i = 0; i < bitSerialisation.Length; i++)
        {
            bitSerialisation[i] = (int)resultArr[i];
        }

        string result = new string(resultArr);
        bitStringEncoding = result;

        return result;
    }

    [ContextMenu("BitDeserialise")]
    public void BitDeserialise()
    {
        //BitDeserialise(bitSerialisation);
        BitDeserialise(bitStringEncoding);
    }


    public void BitDeserialise(string encoding)
    {
        char [] charArr = encoding.ToCharArray();
        int [] intParts = new int[3];

        for(int i = 0; i < intParts.Length; i++)
        {
            intParts[i] = (int)charArr[i];
        }

        BitDeserialise(intParts);
    }

    public void BitDeserialise(int [] parts)
    {
        //extract info from the first part
        byte typeMask = 3;
        byte coreMask = 12;
        byte shellMask = 48;
        byte hMask = 64;
        byte vMask = 128;

        byte type = (byte)((byte)parts[0] & typeMask);
        byte core = (byte)((byte)parts[0] & coreMask);
        byte shell = (byte)((byte)parts[0] & shellMask);
        byte h = (byte)((byte)parts[0] & hMask);
        byte v = (byte)((byte)parts[0] & vMask);

        shellType = (int)(shell>>4);
        coreType = (int)(core>>2);
        horizontalDrag = (h >= 1) ? true : false;
        verticalDrag = (v >= 1) ? true : false;

        //extract info from the second part..
        byte tappableMask = 1;
        byte northMask = 2;
        byte eastMask = 4;
        byte westMask = 8;
        byte southMask = 16;

        byte tp = (byte)(parts[1] & tappableMask); 
        byte n = (byte)(parts[1] & northMask);
        byte e = (byte)(parts[1] & eastMask);
        byte w = (byte)(parts[1] & westMask);
        byte s = (byte)(parts[1] & southMask);

        //Debug.Log("p1: "+ (int)parts[1]);

        tappable = (tp >= 1) ? true : false;
        north = (n >= 1) ? true : false;
        east = (e >= 1) ? true : false;
        west = (w >= 1) ? true : false;
        south = (s >= 1) ? true : false;

        //and also apply coordinates
        if (smoothSnap == null)
            smoothSnap = GetComponent<SmoothSnap>();

        byte xP1Mask = 224;
        byte xP2Mask = 7;
        byte yMask = 248;

        byte xp1 = (byte)(parts[1] & xP1Mask);
        byte xp2 = (byte)(parts[2] & xP2Mask);
        byte x = (byte)((xp1 >> 5) | (xp2 << 3));
        byte y = (byte)((parts[2] & yMask)>>3);
        smoothSnap.gridCoordinates.x = (int)x;
        smoothSnap.gridCoordinates.y = (int)y;
        smoothSnap.InstantSnap();

        //smoothSnap.gridCoordinates.x = (int)parts[2];
        //smoothSnap.gridCoordinates.y = (int)parts[3];
        //smoothSnap.InstantSnap();
    }

    public int LinkType()
    {
        //regular square link
        if (myCrashBolt.opMode == CrashBolt.OperationMode.ShellCrash)
            return 0;

        //triangle link
        if (myCrashBolt.opMode == CrashBolt.OperationMode.ConvertShell)
            return 1;

        //hex link
        if (myCrashBolt.opMode == CrashBolt.OperationMode.TunnelCore)
            return 2;

        return -1;
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

        if (attributes[6].Contains("I"))
            movable = false;

        if (attributes[6].Contains("Z"))
        {
            horizontalDrag = true;
            verticalDrag = false;
        }

        if (attributes[6].Contains("V"))
        {
            verticalDrag = true;
            horizontalDrag = false;
        }

        if (attributes[7].Contains("U"))
            tappable = false;
    }

}

