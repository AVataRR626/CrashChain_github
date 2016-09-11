using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Text))]

public class TextBlink : MonoBehaviour 
{
	public Color col1;
	public Color col2;
	public float col1BlinkRate;
	public float col2BlinkRate;
    public float blinkClock = 3;

	private Text sr;
	private bool blinkFlag;
	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<Text>();
		sr.color = col1;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(blinkClock > 0)
        { 

		    if(sr.color == col1 || sr.color == col2)
			    blinkFlag = !blinkFlag;

		    if(blinkFlag)
		    {
			    sr.color = Color.Lerp (sr.color,col1,col1BlinkRate);
		    }
		    else
		    {
			    sr.color = Color.Lerp (sr.color,col2,col2BlinkRate);
		    }

            blinkClock -= Time.deltaTime;
        }

    }
}
