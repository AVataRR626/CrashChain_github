using UnityEngine;
using System.Collections;


//Matt Cabanag
//
//general function for constant scaling


public class ConstantScale : MonoBehaviour 
{
	public bool zeroLimit = true;
	public Vector3 scaleRate = new Vector3(0.1f,0.1f,0.1f);
	
	private Vector3 delta;
    private Vector3 origScale;
	
	// Use this for initialization
	void Start () 
	{
        origScale = transform.localScale;
    }
    
    void Reset()
    {
        transform.localScale = origScale;
    }
	
	// Update is called once per frame
	void Update () 
	{
		
		delta = scaleRate;
		
		if(zeroLimit)
		{
			if(transform.localScale.x + scaleRate.x <= 0)
			{
				delta.x = 0;
			}
			
			if(transform.localScale.y + scaleRate.y <= 0)
			{
				delta.y = 0;
			}
			
			if(transform.localScale.z + scaleRate.z <= 0)
			{
				delta.x = 0;
			}
		}
		
		transform.localScale += delta;
	}
}
