using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CrashBolt : MonoBehaviour
{
    public enum OperationMode {ShellCrash,ConvertShell,ConvertCore};

    public OperationMode opMode;
    public int type;
    public float charge;
    public float ttl = 10;
    public TypeMaster myTypeMaster;

    // Use this for initialization
    void Start ()
    {
        myTypeMaster = TypeMaster.Instance;
        Destroy(gameObject, ttl);
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
        if (cl != null)
        {
            if (opMode == OperationMode.ShellCrash)
            {
                if (myTypeMaster.typeMatch[type] == cl.shellType)
                {
                    //continue the chain reaction if the shells match..
                    cl.GetComponent<SmoothSnap>().noSnapOverride = true;
                    cl.Kill();

                }
                else
                {

                    //push the crash chain if it doesn't...
                    SmoothSnap snapper = cl.GetComponent<SmoothSnap>();

                    if (Mathf.Abs(cl.transform.position.y - transform.position.y) > 1)
                    { 
                        if (cl.transform.position.y > transform.position.y)
                            snapper.gridCoordinates.y++;
                        else
                            snapper.gridCoordinates.y--;

                        
                     }

                    if(Mathf.Abs(cl.transform.position.x - transform.position.x) > 1)
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

            if (opMode == OperationMode.ConvertShell)
            {
                cl.shellType = type;
            }

            if (opMode == OperationMode.ConvertCore)
            {
                cl.coreType = type;
            }

        }
        Destroy(gameObject);
    }
}
