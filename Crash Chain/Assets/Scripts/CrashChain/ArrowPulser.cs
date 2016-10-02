using UnityEngine;
using System.Collections;

public class ArrowPulser : MonoBehaviour
{
    public static bool pulseMode = true;
    public ArrowPulser instance;

	// Use this for initialization
	void Start ()
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(pulseMode)
        {
            BlockerDisplay[] arrowList = FindObjectsOfType<BlockerDisplay>();

            if (Input.GetMouseButtonDown(0))
            {
                foreach(BlockerDisplay bd in arrowList)
                {
                    bd.BlinkOn();
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                foreach (BlockerDisplay bd in arrowList)
                {
                    bd.BlinkOff();
                }
            }
        }
	}

}
