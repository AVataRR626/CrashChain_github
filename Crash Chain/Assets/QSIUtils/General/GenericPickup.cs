using UnityEngine;
using System.Collections;

public class GenericPickup : MonoBehaviour 
{

	public string type;
	public float value;    

    private Collider col;
    private bool enableSwitch = false;
    private float clock;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (enableSwitch)
        {
            if (clock <= 0)
            {
            
                col.enabled = true;
                enableSwitch = false;
            }
            else
            {
                clock -= Time.deltaTime;
            }
        }

    }

    public void DelayedColliderEnable(float t)
    {
        clock = t;
        enableSwitch = true;
    }


}
