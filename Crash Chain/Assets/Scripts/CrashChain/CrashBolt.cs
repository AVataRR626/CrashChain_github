using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CrashBolt : MonoBehaviour
{
    public enum OperationMode {ShellCrash,ConvertShell,ConvertCore,TunnelCore};

    public OperationMode opMode;
    public int type;
    public float charge;
    public float ttl = 10;
    public TypeMaster myTypeMaster;

    public GameObject[] shellCrashSpawnList;
    public GameObject[] convertShellSpawnList;
    public GameObject[] convertCoreSpawnList;
    public Transform [] detatchOnDeath;

    // Use this for initialization
    void Start ()
    {
        myTypeMaster = TypeMaster.Instance;
        Destroy(gameObject, ttl);

        if(opMode == OperationMode.TunnelCore)
        {
            IgnoreOtherCores(type);
        }

        ttl -= 0.1f;
    }

    void Update()
    {
        if(ttl > 0)
        { 

            ttl -= Time.deltaTime;
        }
        else
        {
            DetatchList();
        }
    }

    void DetatchList()
    {
        foreach (Transform o in detatchOnDeath)
        {
            o.parent = null;

            AutokillTimer autoKiller = o.GetComponent<AutokillTimer>();

            if (autoKiller != null)
                autoKiller.timer = 0.65f;
        }
    }

    public void IgnoreOtherCores(int targetType)
    {
        CrashLink[] allLinks = FindObjectsOfType<CrashLink>();

        Debug.Log("CrashLinkCount: " + allLinks.Length);

        foreach (CrashLink l in allLinks)
        {
            if (l.coreType != targetType)
            {
                Debug.Log("------ ignoring: " + l.name);

                Collider2D otherCol = l.GetComponent<Collider2D>();

                Physics2D.IgnoreCollision(otherCol, GetComponent<Collider2D>(), true);
            }
        }
    }
	
    void OnCollisionEnter2D(Collision2D col)
    {
        //identify the crash link..
        CrashLink cl = col.gameObject.GetComponent<CrashLink>();
        CrashALink(cl);


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        CrashLink cl = col.gameObject.GetComponent<CrashLink>();


        CrashALink(cl);
    }

    void CrashALink(CrashLink cl)
    {
        /*
        LerpToPosition camMover = Camera.main.GetComponent<LerpToPosition>();
        
        if (camMover != null)
        {
            Vector3 newPos = transform.position;
            newPos.z = -1.7f;
            camMover.destination = newPos;
            camMover.lerpTime = 5;
            camMover.lerpClock = 0;
            camMover.moveSwitch = true;
        }*/


        if (cl != null)
        {
            if (opMode == OperationMode.ShellCrash)
            {
                //default shell crashing behaviour..
                ShellCrash(cl);
            }

            if (opMode == OperationMode.TunnelCore)
            {
                //default shell crashing behaviour..
                cl.shellType = type;
                ShellCrash(cl);
            }

            if (opMode == OperationMode.ConvertShell)
            {
                //convert the shell if it doesn't match..
                if (cl.shellType != type)
                {
                    cl.shellType = type;
                    GenUtils.SpawnList(convertShellSpawnList, cl.transform);
                }
                else
                {
                    //if it does match, just do the normal thing
                    ShellCrash(cl);
                }
                
            }

            if (opMode == OperationMode.ConvertCore)
            {
                cl.coreType = type;
            }

        }

        DetatchList();
        Destroy(gameObject);
    }

    void ShellCrash(CrashLink cl)
    {
        

        if (myTypeMaster.typeMatch[type] == cl.shellType)
        {
            GenUtils.SpawnList(shellCrashSpawnList, transform);

            //continue the chain reaction if the shells match..
            cl.GetComponent<SmoothSnap>().noSnapOverride = true;
            //cl.GetComponent<Rigidbody2D>().isKinematic = true;
            cl.Kill();

        }
        else
        {
            if (!cl.movable)
                return;//don't move immovale objects

            //push the crash chain if it doesn't...
            SmoothSnap snapper = cl.GetComponent<SmoothSnap>();

            if (Mathf.Abs(cl.transform.position.y - transform.position.y) > 1)
            {
                if (cl.transform.position.y > transform.position.y)
                    snapper.gridCoordinates.y++;
                else
                    snapper.gridCoordinates.y--;


            }

            if (Mathf.Abs(cl.transform.position.x - transform.position.x) > 1)
            {
                if (cl.transform.position.x > transform.position.x)
                    snapper.gridCoordinates.x++;
                else if (cl.transform.position.x < transform.position.x)
                    snapper.gridCoordinates.x--;
            }

            snapper.anchorGridCoordinates = snapper.gridCoordinates;
            snapper.snapSwitch = true;
            snapper.ManualSnap(snapper.gridCoordinates);

        }
    }
}

